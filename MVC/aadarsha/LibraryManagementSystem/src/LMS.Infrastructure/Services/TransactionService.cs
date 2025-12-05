using LMS.Core.DTOs.Requests;
using LMS.Core.DTOs.Responses;
using LMS.Core.Entities;
using LMS.Core.Interfaces.Repositories;
using LMS.Core.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace LMS.Infrastructure.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IFineService _fineService;
    private readonly ILogger<TransactionService> _logger;

    public TransactionService(
        ITransactionRepository transactionRepository,
        IMemberRepository memberRepository,
        IBookRepository bookRepository,
        IFineService fineService,
        ILogger<TransactionService> logger
    )
    {
        _transactionRepository = transactionRepository;
        _memberRepository = memberRepository;
        _bookRepository = bookRepository;
        _fineService = fineService;
        _logger = logger;
    }

    public async Task<ApiResponse<TransactionResponse>> CheckoutBookAsync(CheckoutRequest request)
    {
        try
        {
            // Check eligibility
            var eligibility = await CheckCheckoutEligibilityAsync(request.MemberId, request.BookId);
            if (!eligibility.Success || !eligibility.Data!.CanCheckout)
            {
                return ApiResponse<TransactionResponse>.ErrorResponse(
                    eligibility.Data?.Message ?? "Not eligible to checkout."
                );
            }

            // Get member and book
            var member = await _memberRepository.GetByIdAsync(request.MemberId);
            var book = await _bookRepository.GetByIdAsync(request.BookId);

            if (member == null || book == null)
            {
                return ApiResponse<TransactionResponse>.ErrorResponse("Member or book not found.");
            }

            // Check book availability
            if (book.AvailableCopies <= 0)
            {
                return ApiResponse<TransactionResponse>.ErrorResponse(
                    "Book is not available for checkout."
                );
            }

            // Create transaction
            var transaction = new Transaction
            {
                MemberId = request.MemberId,
                BookId = request.BookId,
                CheckoutDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(request.LoanPeriodDays),
                Status = "CheckedOut",
                RenewalCount = 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            // Update book availability
            book.AvailableCopies--;
            book.UpdatedAt = DateTime.UtcNow;

            // Update member's checked out count
            member.CurrentBooksCheckedOut++;
            member.UpdatedAt = DateTime.UtcNow;

            await _transactionRepository.AddAsync(transaction);
            await _bookRepository.UpdateAsync(book);
            await _memberRepository.UpdateAsync(member);
            await _transactionRepository.SaveChangesAsync();

            _logger.LogInformation(
                "Book checkout: Transaction {TransactionId}, Member {MemberId}, Book {BookId}",
                transaction.Id,
                request.MemberId,
                request.BookId
            );

            var response = await MapToResponseAsync(transaction);
            return ApiResponse<TransactionResponse>.SuccessResponse(
                response,
                "Book checked out successfully."
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error checking out book for member: {MemberId}",
                request.MemberId
            );
            return ApiResponse<TransactionResponse>.ErrorResponse(
                "An error occurred while checking out book."
            );
        }
    }

    public async Task<ApiResponse<TransactionResponse>> ReturnBookAsync(int transactionId)
    {
        try
        {
            var transaction = await _transactionRepository.GetWithDetailsAsync(transactionId);
            if (transaction == null)
            {
                return ApiResponse<TransactionResponse>.ErrorResponse("Transaction not found.");
            }

            if (transaction.Status == "Returned")
            {
                return ApiResponse<TransactionResponse>.ErrorResponse("Book already returned.");
            }

            transaction.ReturnDate = DateTime.UtcNow;
            transaction.Status = "Returned";
            transaction.UpdatedAt = DateTime.UtcNow;

            decimal lateFee = 0;
            if (transaction.DueDate < DateTime.UtcNow && !transaction.ReturnDate.HasValue)
            {
                var overdueDays = (DateTime.UtcNow - transaction.DueDate).Days;
                lateFee = CalculateLateFee(overdueDays);
                transaction.LateFee = lateFee;
            }

            var book = transaction.Book;
            if (book != null)
            {
                book.AvailableCopies++;
                book.UpdatedAt = DateTime.UtcNow;
                await _bookRepository.UpdateAsync(book);
            }

            var member = transaction.Member;
            if (member != null)
            {
                member.CurrentBooksCheckedOut--;
                member.UpdatedAt = DateTime.UtcNow;
                await _memberRepository.UpdateAsync(member);
            }

            await _transactionRepository.UpdateAsync(transaction);
            await _transactionRepository.SaveChangesAsync();

            _logger.LogInformation(
                "Book returned: Transaction {TransactionId}, Late fee: {LateFee}",
                transactionId,
                lateFee
            );

            var response = await MapToResponseAsync(transaction);
            return ApiResponse<TransactionResponse>.SuccessResponse(
                response,
                lateFee > 0 ? $"Book returned. Late fee: ${lateFee}" : "Book returned successfully."
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error returning book for transaction: {TransactionId}",
                transactionId
            );
            return ApiResponse<TransactionResponse>.ErrorResponse(
                "An error occurred while returning book."
            );
        }
    }

    public async Task<ApiResponse<TransactionResponse>> RenewBookAsync(int transactionId)
    {
        try
        {
            var transaction = await _transactionRepository.GetWithDetailsAsync(transactionId);
            if (transaction == null)
            {
                return ApiResponse<TransactionResponse>.ErrorResponse("Transaction not found.");
            }

            // Check if can be renewed
            if (transaction.Status != "CheckedOut")
            {
                return ApiResponse<TransactionResponse>.ErrorResponse(
                    "Only checked out books can be renewed."
                );
            }

            if (transaction.RenewalCount >= 2) // Max 2 renewals
            {
                return ApiResponse<TransactionResponse>.ErrorResponse("Maximum renewals reached.");
            }

            transaction.DueDate = transaction.DueDate.AddDays(14); // Add 14 more days
            transaction.RenewalCount++;
            transaction.UpdatedAt = DateTime.UtcNow;

            await _transactionRepository.UpdateAsync(transaction);
            await _transactionRepository.SaveChangesAsync();

            _logger.LogInformation(
                "Book renewed: Transaction {TransactionId}, New due date: {DueDate}",
                transactionId,
                transaction.DueDate
            );

            var response = await MapToResponseAsync(transaction);
            return ApiResponse<TransactionResponse>.SuccessResponse(
                response,
                $"Book renewed successfully. New due date: {transaction.DueDate:yyyy-MM-dd}"
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error renewing book for transaction: {TransactionId}",
                transactionId
            );
            return ApiResponse<TransactionResponse>.ErrorResponse(
                "An error occurred while renewing book."
            );
        }
    }

    public async Task<ApiResponse<TransactionResponse>> GetTransactionByIdAsync(int id)
    {
        try
        {
            var transaction = await _transactionRepository.GetWithDetailsAsync(id);
            if (transaction == null)
            {
                return ApiResponse<TransactionResponse>.ErrorResponse("Transaction not found.");
            }

            var response = await MapToResponseAsync(transaction);
            return ApiResponse<TransactionResponse>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching transaction: {TransactionId}", id);
            return ApiResponse<TransactionResponse>.ErrorResponse(
                "An error occurred while fetching transaction."
            );
        }
    }

    public async Task<ApiResponse<IEnumerable<TransactionResponse>>> GetTransactionsByMemberAsync(
        int memberId
    )
    {
        try
        {
            var transactions = await _transactionRepository.GetByMemberIdAsync(memberId);
            var transactionList = transactions.ToList();

            var responses = new List<TransactionResponse>();
            foreach (var transaction in transactionList)
            {
                responses.Add(await MapToResponseAsync(transaction));
            }

            return ApiResponse<IEnumerable<TransactionResponse>>.SuccessResponse(responses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching transactions for member: {MemberId}", memberId);
            return ApiResponse<IEnumerable<TransactionResponse>>.ErrorResponse(
                "An error occurred while fetching transactions."
            );
        }
    }

    public async Task<ApiResponse<IEnumerable<TransactionResponse>>> GetTransactionsByBookAsync(
        int bookId
    )
    {
        try
        {
            var transactions = await _transactionRepository.GetByBookIdAsync(bookId);
            var transactionList = transactions.ToList();

            var responses = new List<TransactionResponse>();
            foreach (var transaction in transactionList)
            {
                responses.Add(await MapToResponseAsync(transaction));
            }

            return ApiResponse<IEnumerable<TransactionResponse>>.SuccessResponse(responses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching transactions for book: {BookId}", bookId);
            return ApiResponse<IEnumerable<TransactionResponse>>.ErrorResponse(
                "An error occurred while fetching transactions."
            );
        }
    }

    public async Task<ApiResponse<IEnumerable<TransactionResponse>>> GetOverdueTransactionsAsync()
    {
        try
        {
            var transactions = await _transactionRepository.GetOverdueTransactionsAsync();
            var transactionList = transactions.ToList();

            var responses = new List<TransactionResponse>();
            foreach (var transaction in transactionList)
            {
                responses.Add(await MapToResponseAsync(transaction));
            }

            return ApiResponse<IEnumerable<TransactionResponse>>.SuccessResponse(responses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching overdue transactions");
            return ApiResponse<IEnumerable<TransactionResponse>>.ErrorResponse(
                "An error occurred while fetching overdue transactions."
            );
        }
    }

    public async Task<ApiResponse<IEnumerable<TransactionResponse>>> GetActiveCheckoutsAsync(
        int memberId
    )
    {
        try
        {
            var transactions = await _transactionRepository.GetActiveCheckoutsAsync(memberId);
            var transactionList = transactions.ToList();

            var responses = new List<TransactionResponse>();
            foreach (var transaction in transactionList)
            {
                responses.Add(await MapToResponseAsync(transaction));
            }

            return ApiResponse<IEnumerable<TransactionResponse>>.SuccessResponse(responses);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error fetching active checkouts for member: {MemberId}",
                memberId
            );
            return ApiResponse<IEnumerable<TransactionResponse>>.ErrorResponse(
                "An error occurred while fetching active checkouts."
            );
        }
    }

    public async Task<ApiResponse<CheckoutEligibilityResponse>> CheckCheckoutEligibilityAsync(
        int memberId,
        int bookId
    )
    {
        try
        {
            var member = await _memberRepository.GetWithDetailsAsync(memberId);
            var book = await _bookRepository.GetByIdAsync(bookId);

            if (member == null)
            {
                return ApiResponse<CheckoutEligibilityResponse>.ErrorResponse("Member not found.");
            }

            if (book == null)
            {
                return ApiResponse<CheckoutEligibilityResponse>.ErrorResponse("Book not found.");
            }

            // Check member status
            if (!member.IsMembershipActive)
            {
                return ApiResponse<CheckoutEligibilityResponse>.SuccessResponse(
                    new CheckoutEligibilityResponse
                    {
                        CanCheckout = false,
                        Message = "Member account is inactive.",
                        IsMemberActive = false,
                    }
                );
            }

            // Check max books limit
            if (member.CurrentBooksCheckedOut >= member.MaxBooksAllowed)
            {
                return ApiResponse<CheckoutEligibilityResponse>.SuccessResponse(
                    new CheckoutEligibilityResponse
                    {
                        CanCheckout = false,
                        Message = $"Maximum books limit reached ({member.MaxBooksAllowed}).",
                        BooksCheckedOut = member.CurrentBooksCheckedOut,
                        MaxBooksAllowed = member.MaxBooksAllowed,
                    }
                );
            }

            // Check pending fines
            var pendingFines = member
                .Fines.Where(f => f.Status == "Pending" && f.IsActive)
                .Sum(f => f.Amount);

            if (pendingFines > 0)
            {
                return ApiResponse<CheckoutEligibilityResponse>.SuccessResponse(
                    new CheckoutEligibilityResponse
                    {
                        CanCheckout = false,
                        Message = $"Member has pending fines: ${pendingFines}.",
                        PendingFines = pendingFines,
                    }
                );
            }

            // Check for overdue books
            var hasOverdue = member.Transactions.Any(t =>
                t.Status == "CheckedOut" && t.DueDate < DateTime.UtcNow && t.ReturnDate == null
            );

            if (hasOverdue)
            {
                return ApiResponse<CheckoutEligibilityResponse>.SuccessResponse(
                    new CheckoutEligibilityResponse
                    {
                        CanCheckout = false,
                        Message = "Member has overdue books.",
                        HasOverdueBooks = true,
                    }
                );
            }

            // Check book availability
            if (book.AvailableCopies <= 0)
            {
                return ApiResponse<CheckoutEligibilityResponse>.SuccessResponse(
                    new CheckoutEligibilityResponse
                    {
                        CanCheckout = false,
                        Message = "Book is not available.",
                        IsBookAvailable = false,
                    }
                );
            }

            // All checks passed
            return ApiResponse<CheckoutEligibilityResponse>.SuccessResponse(
                new CheckoutEligibilityResponse
                {
                    CanCheckout = true,
                    Message = "Eligible to checkout.",
                    BooksCheckedOut = member.CurrentBooksCheckedOut,
                    MaxBooksAllowed = member.MaxBooksAllowed,
                    PendingFines = pendingFines,
                    HasOverdueBooks = hasOverdue,
                    IsMemberActive = true,
                    IsBookAvailable = true,
                }
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error checking checkout eligibility for member: {MemberId}",
                memberId
            );
            return ApiResponse<CheckoutEligibilityResponse>.ErrorResponse(
                "An error occurred while checking eligibility."
            );
        }
    }

    public async Task<ApiResponse<TransactionStatsResponse>> GetTransactionStatsAsync(
        DateTime startDate,
        DateTime endDate
    )
    {
        try
        {
            var transactions = await _transactionRepository.GetTransactionsByDateRangeAsync(
                startDate,
                endDate
            );
            var transactionList = transactions.ToList();

            var stats = new TransactionStatsResponse
            {
                StartDate = startDate,
                EndDate = endDate,
                TotalCheckouts = transactionList.Count(t =>
                    t.CheckoutDate >= startDate && t.CheckoutDate <= endDate
                ),
                TotalReturns = transactionList.Count(t =>
                    t.ReturnDate.HasValue && t.ReturnDate >= startDate && t.ReturnDate <= endDate
                ),
                TotalRenewals = transactionList.Sum(t => t.RenewalCount ?? 0),
                OverdueCount = transactionList.Count(t =>
                    t.DueDate < DateTime.UtcNow && !t.ReturnDate.HasValue
                ),
                TotalFinesCollected = transactionList.Sum(t => t.LateFee ?? 0m),
            };

            // Group checkouts by day
            var checkoutsByDay = transactionList
                .Where(t => t.CheckoutDate >= startDate && t.CheckoutDate <= endDate)
                .GroupBy(t => t.CheckoutDate.Date)
                .ToDictionary(g => g.Key.ToString("yyyy-MM-dd"), g => g.Count());

            stats.CheckoutsByDay = checkoutsByDay;

            return ApiResponse<TransactionStatsResponse>.SuccessResponse(stats);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching transaction stats");
            return ApiResponse<TransactionStatsResponse>.ErrorResponse(
                "An error occurred while fetching transaction statistics."
            );
        }
    }

    private decimal CalculateLateFee(int overdueDays)
    {
        const decimal dailyFee = 0.50m;
        const decimal maxFee = 10.00m;

        var fee = overdueDays * dailyFee;
        return Math.Min(fee, maxFee);
    }

    private async Task<TransactionResponse> MapToResponseAsync(Transaction transaction)
    {
        var isOverdue =
            transaction.Status == "CheckedOut"
            && transaction.DueDate < DateTime.UtcNow
            && !transaction.ReturnDate.HasValue;

        var overdueDays = isOverdue ? (DateTime.UtcNow - transaction.DueDate).Days : 0;

        return new TransactionResponse
        {
            Id = transaction.Id,
            MemberId = transaction.MemberId,
            MemberName =
                transaction.Member?.User != null
                    ? $"{transaction.Member.User.FirstName} {transaction.Member.User.LastName}"
                    : "Unknown",
            BookId = transaction.BookId,
            BookTitle = transaction.Book?.Title ?? "Unknown",
            CheckoutDate = transaction.CheckoutDate,
            DueDate = transaction.DueDate,
            ReturnDate = transaction.ReturnDate,
            Status = transaction.Status,
            LateFee = transaction.LateFee,
            RenewalCount = transaction.RenewalCount ?? 0,
            IsOverdue = isOverdue,
            OverdueDays = overdueDays,
            CreatedAt = transaction.CreatedAt,
        };
    }
}
