using LMS.Core.DTOs.Requests;
using LMS.Core.DTOs.Responses;
using LMS.Core.Entities;
using LMS.Core.Interfaces.Repositories;
using LMS.Core.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace LMS.Infrastructure.Services;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IBookRepository _bookRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ILogger<ReservationService> _logger;

    public ReservationService(
        IReservationRepository reservationRepository,
        IMemberRepository memberRepository,
        IBookRepository bookRepository,
        ITransactionRepository transactionRepository,
        ILogger<ReservationService> logger
    )
    {
        _reservationRepository = reservationRepository;
        _memberRepository = memberRepository;
        _bookRepository = bookRepository;
        _transactionRepository = transactionRepository;
        _logger = logger;
    }

    public async Task<ApiResponse<ReservationResponse>> CreateReservationAsync(
        CreateReservationRequest request
    )
    {
        try
        {
            // Check eligibility
            var eligibility = await CheckReservationEligibilityAsync(
                request.MemberId,
                request.BookId
            );
            if (!eligibility.Success || !eligibility.Data)
            {
                return ApiResponse<ReservationResponse>.ErrorResponse(
                    "Not eligible to reserve this book."
                );
            }

            var member = await _memberRepository.GetByIdAsync(request.MemberId);
            var book = await _bookRepository.GetByIdAsync(request.BookId);

            if (member == null || book == null)
            {
                return ApiResponse<ReservationResponse>.ErrorResponse("Member or book not found.");
            }

            // Get queue position
            var activeReservations = await _reservationRepository.GetActiveReservationsAsync(
                request.BookId
            );
            var queuePosition = activeReservations.Count() + 1;

            // Create reservation
            var reservation = new Reservation
            {
                MemberId = request.MemberId,
                BookId = request.BookId,
                ReservationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(request.ExpiryDays),
                Status = "Pending",
                PositionInQueue = queuePosition,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            await _reservationRepository.AddAsync(reservation);
            await _reservationRepository.SaveChangesAsync();

            _logger.LogInformation(
                "Reservation created: {ReservationId}, Member: {MemberId}, Book: {BookId}, Position: {Position}",
                reservation.Id,
                request.MemberId,
                request.BookId,
                queuePosition
            );

            var response = await MapToResponseAsync(reservation);
            return ApiResponse<ReservationResponse>.SuccessResponse(
                response,
                $"Reservation created successfully. Position in queue: {queuePosition}"
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error creating reservation for member: {MemberId}",
                request.MemberId
            );
            return ApiResponse<ReservationResponse>.ErrorResponse(
                "An error occurred while creating reservation."
            );
        }
    }

    public async Task<ApiResponse<ReservationResponse>> GetReservationByIdAsync(int id)
    {
        try
        {
            var reservation = await _reservationRepository.GetWithDetailsAsync(id);
            if (reservation == null)
            {
                return ApiResponse<ReservationResponse>.ErrorResponse("Reservation not found.");
            }

            var response = await MapToResponseAsync(reservation);
            return ApiResponse<ReservationResponse>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching reservation: {ReservationId}", id);
            return ApiResponse<ReservationResponse>.ErrorResponse(
                "An error occurred while fetching reservation."
            );
        }
    }

    public async Task<ApiResponse<IEnumerable<ReservationResponse>>> GetReservationsByMemberAsync(
        int memberId
    )
    {
        try
        {
            var reservations = await _reservationRepository.GetByMemberIdAsync(memberId);
            var reservationList = reservations.ToList();

            var responses = new List<ReservationResponse>();
            foreach (var reservation in reservationList)
            {
                responses.Add(await MapToResponseAsync(reservation));
            }

            return ApiResponse<IEnumerable<ReservationResponse>>.SuccessResponse(responses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching reservations for member: {MemberId}", memberId);
            return ApiResponse<IEnumerable<ReservationResponse>>.ErrorResponse(
                "An error occurred while fetching reservations."
            );
        }
    }

    public async Task<ApiResponse<IEnumerable<ReservationResponse>>> GetReservationsByBookAsync(
        int bookId
    )
    {
        try
        {
            var reservations = await _reservationRepository.GetByBookIdAsync(bookId);
            var reservationList = reservations.ToList();

            var responses = new List<ReservationResponse>();
            foreach (var reservation in reservationList)
            {
                responses.Add(await MapToResponseAsync(reservation));
            }

            return ApiResponse<IEnumerable<ReservationResponse>>.SuccessResponse(responses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching reservations for book: {BookId}", bookId);
            return ApiResponse<IEnumerable<ReservationResponse>>.ErrorResponse(
                "An error occurred while fetching reservations."
            );
        }
    }

    public async Task<ApiResponse<ReservationResponse>> CancelReservationAsync(int reservationId)
    {
        try
        {
            var reservation = await _reservationRepository.GetWithDetailsAsync(reservationId);
            if (reservation == null)
            {
                return ApiResponse<ReservationResponse>.ErrorResponse("Reservation not found.");
            }

            if (reservation.Status == "Cancelled" || reservation.Status == "Fulfilled")
            {
                return ApiResponse<ReservationResponse>.ErrorResponse(
                    $"Reservation already {reservation.Status.ToLower()}."
                );
            }

            reservation.Status = "Cancelled";
            reservation.UpdatedAt = DateTime.UtcNow;

            await _reservationRepository.UpdateAsync(reservation);
            await _reservationRepository.SaveChangesAsync();

            // Update queue positions for remaining reservations
            await UpdateQueuePositionsAsync(reservation.BookId);

            _logger.LogInformation("Reservation cancelled: {ReservationId}", reservationId);

            var response = await MapToResponseAsync(reservation);
            return ApiResponse<ReservationResponse>.SuccessResponse(
                response,
                "Reservation cancelled successfully."
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling reservation: {ReservationId}", reservationId);
            return ApiResponse<ReservationResponse>.ErrorResponse(
                "An error occurred while cancelling reservation."
            );
        }
    }

    public async Task<ApiResponse<ReservationResponse>> FulfillReservationAsync(int reservationId)
    {
        try
        {
            var reservation = await _reservationRepository.GetWithDetailsAsync(reservationId);
            if (reservation == null)
            {
                return ApiResponse<ReservationResponse>.ErrorResponse("Reservation not found.");
            }

            if (reservation.Status != "Available")
            {
                return ApiResponse<ReservationResponse>.ErrorResponse(
                    "Reservation is not available for fulfillment."
                );
            }

            reservation.Status = "Fulfilled";
            reservation.UpdatedAt = DateTime.UtcNow;

            await _reservationRepository.UpdateAsync(reservation);
            await _reservationRepository.SaveChangesAsync();

            _logger.LogInformation("Reservation fulfilled: {ReservationId}", reservationId);

            var response = await MapToResponseAsync(reservation);
            return ApiResponse<ReservationResponse>.SuccessResponse(
                response,
                "Reservation fulfilled successfully."
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fulfilling reservation: {ReservationId}", reservationId);
            return ApiResponse<ReservationResponse>.ErrorResponse(
                "An error occurred while fulfilling reservation."
            );
        }
    }

    public async Task<ApiResponse<bool>> NotifyNextInQueueAsync(int bookId)
    {
        try
        {
            var nextReservation = (
                await _reservationRepository.GetActiveReservationsAsync(bookId)
            ).FirstOrDefault(r => r.Status == "Pending");

            if (nextReservation == null)
            {
                return ApiResponse<bool>.SuccessResponse(
                    false,
                    "No pending reservations for this book."
                );
            }

            // Update reservation status to "Available"
            nextReservation.Status = "Available";
            nextReservation.NotificationDate = DateTime.UtcNow;
            nextReservation.UpdatedAt = DateTime.UtcNow;

            await _reservationRepository.UpdateAsync(nextReservation);
            await _reservationRepository.SaveChangesAsync();

            _logger.LogInformation(
                "Notified next in queue: Reservation {ReservationId}, Member: {MemberId}",
                nextReservation.Id,
                nextReservation.MemberId
            );

            // Here you would typically send an email or notification
            // await _notificationService.SendReservationAvailableNotification(nextReservation);

            return ApiResponse<bool>.SuccessResponse(true, "Next in queue notified successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error notifying next in queue for book: {BookId}", bookId);
            return ApiResponse<bool>.ErrorResponse(
                "An error occurred while notifying next in queue."
            );
        }
    }

    public async Task<ApiResponse<int>> GetQueuePositionAsync(int reservationId)
    {
        try
        {
            var reservation = await _reservationRepository.GetByIdAsync(reservationId);
            if (reservation == null)
            {
                return ApiResponse<int>.ErrorResponse("Reservation not found.");
            }

            var position = await _reservationRepository.GetQueuePositionAsync(
                reservation.BookId,
                reservationId
            );
            return ApiResponse<int>.SuccessResponse(position);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error getting queue position for reservation: {ReservationId}",
                reservationId
            );
            return ApiResponse<int>.ErrorResponse(
                "An error occurred while getting queue position."
            );
        }
    }

    public async Task<ApiResponse<bool>> CheckReservationEligibilityAsync(int memberId, int bookId)
    {
        try
        {
            var member = await _memberRepository.GetWithDetailsAsync(memberId);
            var book = await _bookRepository.GetByIdAsync(bookId);

            if (member == null)
            {
                return ApiResponse<bool>.ErrorResponse("Member not found.");
            }

            if (book == null)
            {
                return ApiResponse<bool>.ErrorResponse("Book not found.");
            }

            // Check member status
            if (!member.IsMembershipActive)
            {
                return ApiResponse<bool>.SuccessResponse(false);
            }

            // Check if member already has an active reservation for this book
            var hasActiveReservation = await _reservationRepository.HasActiveReservationAsync(
                memberId,
                bookId
            );
            if (hasActiveReservation)
            {
                return ApiResponse<bool>.SuccessResponse(false);
            }

            // Check if member already has this book checked out
            var activeCheckout = await _transactionRepository.GetActiveCheckoutAsync(
                memberId,
                bookId
            );
            if (activeCheckout != null)
            {
                return ApiResponse<bool>.SuccessResponse(false);
            }

            // Check if book has available copies (no need to reserve if available)
            if (book.AvailableCopies > 0)
            {
                return ApiResponse<bool>.SuccessResponse(
                    false,
                    "Book is available for immediate checkout."
                );
            }

            return ApiResponse<bool>.SuccessResponse(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error checking reservation eligibility for member: {MemberId}",
                memberId
            );
            return ApiResponse<bool>.ErrorResponse("An error occurred while checking eligibility.");
        }
    }

    private async Task UpdateQueuePositionsAsync(int bookId)
    {
        var activeReservations = (await _reservationRepository.GetActiveReservationsAsync(bookId))
            .OrderBy(r => r.PositionInQueue)
            .ThenBy(r => r.ReservationDate)
            .ToList();

        for (int i = 0; i < activeReservations.Count; i++)
        {
            activeReservations[i].PositionInQueue = i + 1;
            activeReservations[i].UpdatedAt = DateTime.UtcNow;
            await _reservationRepository.UpdateAsync(activeReservations[i]);
        }

        await _reservationRepository.SaveChangesAsync();
    }

    private async Task<ReservationResponse> MapToResponseAsync(Reservation reservation)
    {
        var isActive = reservation.Status == "Pending" || reservation.Status == "Available";

        return new ReservationResponse
        {
            Id = reservation.Id,
            MemberId = reservation.MemberId,
            MemberName =
                reservation.Member?.User != null
                    ? $"{reservation.Member.User.FirstName} {reservation.Member.User.LastName}"
                    : "Unknown",
            BookId = reservation.BookId,
            BookTitle = reservation.Book?.Title ?? "Unknown",
            ReservationDate = reservation.ReservationDate,
            ExpiryDate = reservation.ExpiryDate,
            NotificationDate = reservation.NotificationDate,
            Status = reservation.Status,
            PositionInQueue = reservation.PositionInQueue,
            IsActive = isActive,
            CreatedAt = reservation.CreatedAt,
        };
    }
}
