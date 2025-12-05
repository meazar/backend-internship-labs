using LMS.Core.DTOs.Requests;
using LMS.Core.DTOs.Responses;
using LMS.Core.Entities;
using LMS.Core.Interfaces.Repositories;
using LMS.Core.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace LMS.Infrastructure.Services;

public class MemberService : IMemberService
{
    private readonly IMemberRepository _memberRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<MemberService> _logger;

    public MemberService(
        IMemberRepository memberRepository,
        IUserRepository userRepository,
        ILogger<MemberService> logger
    )
    {
        _memberRepository = memberRepository;
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<ApiResponse<MemberResponse>> CreateMemberAsync(int userId)
    {
        try
        {
            // Check if user exists
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return ApiResponse<MemberResponse>.ErrorResponse("User not found.");
            }

            // Check if member already exists for this user
            var existingMember = await _memberRepository.GetByUserIdAsync(userId);
            if (existingMember != null)
            {
                return ApiResponse<MemberResponse>.ErrorResponse(
                    "Member already exists for this user."
                );
            }

            // Generate unique member ID
            var memberId = GenerateMemberId(user);

            // Create new member
            var member = new Member
            {
                UserId = userId,
                MemberId = memberId,
                MembershipStartDate = DateTime.UtcNow,
                MembershipType = "Regular",
                MaxBooksAllowed = 5,
                CurrentBooksCheckedOut = 0,
                TotalFines = 0,
                IsMembershipActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            await _memberRepository.AddAsync(member);
            await _memberRepository.SaveChangesAsync();

            _logger.LogInformation(
                "Member created: {MemberId} for User: {UserId}",
                memberId,
                userId
            );

            var response = MapToResponse(member, user);
            return ApiResponse<MemberResponse>.SuccessResponse(
                response,
                "Member created successfully."
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating member for user: {UserId}", userId);
            return ApiResponse<MemberResponse>.ErrorResponse(
                "An error occurred while creating member."
            );
        }
    }

    public async Task<ApiResponse<MemberResponse>> GetMemberByIdAsync(int id)
    {
        try
        {
            var member = await _memberRepository.GetWithDetailsAsync(id);
            if (member == null)
            {
                return ApiResponse<MemberResponse>.ErrorResponse("Member not found.");
            }

            var response = MapToResponse(member, member.User);
            return ApiResponse<MemberResponse>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching member: {MemberId}", id);
            return ApiResponse<MemberResponse>.ErrorResponse(
                "An error occurred while fetching member."
            );
        }
    }

    public async Task<ApiResponse<MemberResponse>> GetMemberByUserIdAsync(int userId)
    {
        try
        {
            var member = await _memberRepository.GetByUserIdAsync(userId);
            if (member == null)
            {
                return ApiResponse<MemberResponse>.ErrorResponse("Member not found for this user.");
            }

            var response = MapToResponse(member, member.User);
            return ApiResponse<MemberResponse>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching member by user: {UserId}", userId);
            return ApiResponse<MemberResponse>.ErrorResponse(
                "An error occurred while fetching member."
            );
        }
    }

    public async Task<ApiResponse<MemberResponse>> GetMemberByMemberIdAsync(string memberId)
    {
        try
        {
            var member = await _memberRepository.GetByMemberIdAsync(memberId);
            if (member == null)
            {
                return ApiResponse<MemberResponse>.ErrorResponse("Member not found.");
            }

            var response = MapToResponse(member, member.User);
            return ApiResponse<MemberResponse>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching member: {MemberId}", memberId);
            return ApiResponse<MemberResponse>.ErrorResponse(
                "An error occurred while fetching member."
            );
        }
    }

    public async Task<ApiResponse<IEnumerable<MemberResponse>>> GetAllMembersAsync()
    {
        try
        {
            var members = await _memberRepository.GetAllAsync();
            var memberList = members.ToList();

            var responses = memberList.Select(m => MapToResponse(m, m.User)).ToList();
            return ApiResponse<IEnumerable<MemberResponse>>.SuccessResponse(responses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all members");
            return ApiResponse<IEnumerable<MemberResponse>>.ErrorResponse(
                "An error occurred while fetching members."
            );
        }
    }

    public async Task<ApiResponse<IEnumerable<MemberResponse>>> GetActiveMembersAsync()
    {
        try
        {
            var members = await _memberRepository.GetActiveMembersAsync();
            var responses = members.Select(m => MapToResponse(m, m.User)).ToList();
            return ApiResponse<IEnumerable<MemberResponse>>.SuccessResponse(responses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching active members");
            return ApiResponse<IEnumerable<MemberResponse>>.ErrorResponse(
                "An error occurred while fetching active members."
            );
        }
    }

    public async Task<ApiResponse<MemberResponse>> UpdateMemberAsync(
        int id,
        UpdateMemberRequest request
    )
    {
        try
        {
            var member = await _memberRepository.GetByIdAsync(id);
            if (member == null)
            {
                return ApiResponse<MemberResponse>.ErrorResponse("Member not found.");
            }

            // Update fields if provided
            if (!string.IsNullOrEmpty(request.MemberId))
            {
                // Check if new member ID is unique
                if (request.MemberId != member.MemberId)
                {
                    var exists = await _memberRepository.IsMemberIdExistsAsync(request.MemberId);
                    if (exists)
                    {
                        return ApiResponse<MemberResponse>.ErrorResponse(
                            "Member ID already exists."
                        );
                    }
                    member.MemberId = request.MemberId;
                }
            }

            if (!string.IsNullOrEmpty(request.MembershipType))
                member.MembershipType = request.MembershipType;

            if (request.MaxBooksAllowed.HasValue)
                member.MaxBooksAllowed = request.MaxBooksAllowed.Value;

            if (request.IsMembershipActive.HasValue)
                member.IsMembershipActive = request.IsMembershipActive.Value;

            member.UpdatedAt = DateTime.UtcNow;

            await _memberRepository.UpdateAsync(member);
            await _memberRepository.SaveChangesAsync();

            var updatedMember = await _memberRepository.GetWithDetailsAsync(id);
            var response = MapToResponse(updatedMember!, updatedMember!.User);
            return ApiResponse<MemberResponse>.SuccessResponse(
                response,
                "Member updated successfully."
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating member: {MemberId}", id);
            return ApiResponse<MemberResponse>.ErrorResponse(
                "An error occurred while updating member."
            );
        }
    }

    public async Task<ApiResponse<bool>> DeactivateMemberAsync(int id)
    {
        try
        {
            var member = await _memberRepository.GetByIdAsync(id);
            if (member == null)
            {
                return ApiResponse<bool>.ErrorResponse("Member not found.");
            }

            // Check if member has active checkouts
            var activeCheckouts = await _memberRepository.GetCheckedOutCountAsync(id);
            if (activeCheckouts > 0)
            {
                return ApiResponse<bool>.ErrorResponse(
                    "Cannot deactivate member with active book checkouts."
                );
            }

            member.IsMembershipActive = false;
            member.UpdatedAt = DateTime.UtcNow;

            await _memberRepository.UpdateAsync(member);
            await _memberRepository.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Member deactivated successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deactivating member: {MemberId}", id);
            return ApiResponse<bool>.ErrorResponse("An error occurred while deactivating member.");
        }
    }

    public async Task<ApiResponse<bool>> ActivateMemberAsync(int id)
    {
        try
        {
            var member = await _memberRepository.GetByIdAsync(id);
            if (member == null)
            {
                return ApiResponse<bool>.ErrorResponse("Member not found.");
            }

            member.IsMembershipActive = true;
            member.UpdatedAt = DateTime.UtcNow;

            await _memberRepository.UpdateAsync(member);
            await _memberRepository.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Member activated successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error activating member: {MemberId}", id);
            return ApiResponse<bool>.ErrorResponse("An error occurred while activating member.");
        }
    }

    public async Task<ApiResponse<MemberStatusResponse>> GetMemberStatusAsync(int memberId)
    {
        try
        {
            var member = await _memberRepository.GetWithDetailsAsync(memberId);
            if (member == null)
            {
                return ApiResponse<MemberStatusResponse>.ErrorResponse("Member not found.");
            }

            // Get active checkouts count
            var activeCheckouts = await _memberRepository.GetCheckedOutCountAsync(memberId);

            // Get pending fines total
            var pendingFines = member
                .Fines.Where(f => f.Status == "Pending" && f.IsActive)
                .Sum(f => f.Amount);

            // Check for overdue books
            var hasOverdue = member.Transactions.Any(t =>
                t.Status == "CheckedOut" && t.DueDate < DateTime.UtcNow && t.ReturnDate == null
            );

            // Get active reservations
            var activeReservations = member.Reservations.Count(r =>
                r.Status == "Pending" || r.Status == "Available"
            );

            var response = new MemberStatusResponse
            {
                MemberId = memberId,
                CanBorrow =
                    member.IsMembershipActive
                    && activeCheckouts < member.MaxBooksAllowed
                    && pendingFines == 0
                    && !hasOverdue,
                Status = member.IsMembershipActive ? "Active" : "Inactive",
                BooksCheckedOut = activeCheckouts,
                BooksAllowed = member.MaxBooksAllowed,
                PendingFines = pendingFines,
                HasOverdueBooks = hasOverdue,
                ActiveReservations = activeReservations,
            };

            return ApiResponse<MemberStatusResponse>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting member status: {MemberId}", memberId);
            return ApiResponse<MemberStatusResponse>.ErrorResponse(
                "An error occurred while getting member status."
            );
        }
    }

    public async Task<ApiResponse<bool>> CanBorrowMoreBooksAsync(int memberId)
    {
        try
        {
            var statusResponse = await GetMemberStatusAsync(memberId);
            if (!statusResponse.Success)
            {
                return ApiResponse<bool>.ErrorResponse(statusResponse.Message);
            }

            return ApiResponse<bool>.SuccessResponse(statusResponse.Data!.CanBorrow);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking borrow eligibility: {MemberId}", memberId);
            return ApiResponse<bool>.ErrorResponse(
                "An error occurred while checking borrow eligibility."
            );
        }
    }

    // Helper methods
    private string GenerateMemberId(User user)
    {
        var prefix = "LIB";
        var year = DateTime.UtcNow.Year % 100;
        var random = new Random().Next(1000, 9999);
        var initials = $"{user.FirstName[0]}{user.LastName[0]}".ToUpper();

        return $"{prefix}{year}{random}{initials}";
    }

    private MemberResponse MapToResponse(Member member, User user)
    {
        return new MemberResponse
        {
            Id = member.Id,
            UserId = member.UserId,
            MemberId = member.MemberId,
            UserName = user.Username,
            Email = user.Email,
            FullName = $"{user.FirstName} {user.LastName}",
            MembershipStartDate = member.MembershipStartDate,
            MembershipEndDate = member.MembershipEndDate,
            MembershipType = member.MembershipType ?? string.Empty,
            MaxBooksAllowed = member.MaxBooksAllowed,
            CurrentBooksCheckedOut = member.CurrentBooksCheckedOut,
            TotalFines = member.TotalFines,
            IsMembershipActive = member.IsMembershipActive,
            CreatedAt = member.CreatedAt,
            UpdatedAt = member.UpdatedAt,
        };
    }
}
