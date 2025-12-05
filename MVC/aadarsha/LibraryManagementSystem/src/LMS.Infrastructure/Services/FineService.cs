using LMS.Core.DTOs.Requests;
using LMS.Core.DTOs.Responses;
using LMS.Core.Entities;
using LMS.Core.Interfaces.Repositories;
using LMS.Core.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace LMS.Infrastructure.Services
{
    public class FineService : IFineService
    {
        private readonly IFineRepository _fineRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ILogger<FineService> _logger;

        public FineService(
            IFineRepository fineRepository,
            IMemberRepository memberRepository,
            ITransactionRepository transactionRepository,
            ILogger<FineService> logger
        )
        {
            _fineRepository = fineRepository;
            _memberRepository = memberRepository;
            _transactionRepository = transactionRepository;
            _logger = logger;
        }

        public async Task<ApiResponse<FineResponse>> CreateFineAsync(CreateFineRequest request)
        {
            try
            {
                // Validate member
                var member = await _memberRepository.GetByIdAsync(request.MemberId);
                if (member == null)
                {
                    return ApiResponse<FineResponse>.ErrorResponse("Member not found.");
                }

                // Validate transaction if provided
                Transaction? transaction = null;
                if (request.TransactionId.HasValue)
                {
                    transaction = await _transactionRepository.GetByIdAsync(
                        request.TransactionId.Value
                    );
                    if (transaction == null)
                    {
                        return ApiResponse<FineResponse>.ErrorResponse("Transaction not found.");
                    }
                }

                // Create fine
                var fine = new Fine
                {
                    MemberId = request.MemberId,
                    TransactionId = request.TransactionId,
                    Amount = request.Amount,
                    Reason = request.Reason,
                    Description = request.Description,
                    IssueDate = DateTime.UtcNow,
                    DueDate = DateTime.UtcNow.AddDays(30), // 30 days to pay
                    Status = "Pending",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };

                await _fineRepository.AddAsync(fine);
                await _fineRepository.SaveChangesAsync();

                // Update member's total fines
                member.TotalFines += request.Amount;
                await _memberRepository.UpdateAsync(member);

                _logger.LogInformation(
                    "Fine created: {FineId} for Member: {MemberId}, Amount: {Amount}",
                    fine.Id,
                    request.MemberId,
                    request.Amount
                );

                var response = await MapToResponseAsync(fine);
                return ApiResponse<FineResponse>.SuccessResponse(
                    response,
                    "Fine created successfully."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error creating fine for member: {MemberId}",
                    request.MemberId
                );
                return ApiResponse<FineResponse>.ErrorResponse(
                    "An error occurred while creating fine."
                );
            }
        }

        public async Task<ApiResponse<FineResponse>> GetFineByIdAsync(int id)
        {
            try
            {
                var fine = await _fineRepository.GetWithDetailsAsync(id);
                if (fine == null)
                {
                    return ApiResponse<FineResponse>.ErrorResponse("Fine not found.");
                }

                var response = await MapToResponseAsync(fine);
                return ApiResponse<FineResponse>.SuccessResponse(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching fine: {FineId}", id);
                return ApiResponse<FineResponse>.ErrorResponse(
                    "An error occurred while fetching fine."
                );
            }
        }

        public async Task<ApiResponse<IEnumerable<FineResponse>>> GetFinesByMemberAsync(
            int memberId
        )
        {
            try
            {
                var fines = await _fineRepository.GetByMemberIdAsync(memberId);
                var fineList = fines.ToList();

                var responses = new List<FineResponse>();
                foreach (var fine in fineList)
                {
                    responses.Add(await MapToResponseAsync(fine));
                }

                return ApiResponse<IEnumerable<FineResponse>>.SuccessResponse(responses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching fines for member: {MemberId}", memberId);
                return ApiResponse<IEnumerable<FineResponse>>.ErrorResponse(
                    "An error occurred while fetching fines."
                );
            }
        }

        public async Task<ApiResponse<IEnumerable<FineResponse>>> GetPendingFinesAsync()
        {
            try
            {
                var fines = await _fineRepository.GetPendingFinesAsync();
                var fineList = fines.ToList();

                var responses = new List<FineResponse>();
                foreach (var fine in fineList)
                {
                    responses.Add(await MapToResponseAsync(fine));
                }

                return ApiResponse<IEnumerable<FineResponse>>.SuccessResponse(responses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching pending fines");
                return ApiResponse<IEnumerable<FineResponse>>.ErrorResponse(
                    "An error occurred while fetching pending fines."
                );
            }
        }

        public async Task<ApiResponse<FineResponse>> PayFineAsync(int fineId, decimal amount)
        {
            try
            {
                var fine = await _fineRepository.GetWithDetailsAsync(fineId);
                if (fine == null)
                {
                    return ApiResponse<FineResponse>.ErrorResponse("Fine not found.");
                }

                if (fine.Status == "Paid")
                {
                    return ApiResponse<FineResponse>.ErrorResponse("Fine already paid.");
                }

                if (amount <= 0 || amount > fine.Amount)
                {
                    return ApiResponse<FineResponse>.ErrorResponse(
                        $"Invalid payment amount. Amount due: ${fine.Amount}"
                    );
                }

                // Update fine
                fine.Amount -= amount;

                if (fine.Amount <= 0)
                {
                    fine.Status = "Paid";
                    fine.PaymentDate = DateTime.UtcNow;
                }

                fine.UpdatedAt = DateTime.UtcNow;

                // Update member's total fines
                if (fine.Member != null)
                {
                    fine.Member.TotalFines -= amount;
                    await _memberRepository.UpdateAsync(fine.Member);
                }

                await _fineRepository.UpdateAsync(fine);
                await _fineRepository.SaveChangesAsync();

                _logger.LogInformation(
                    "Fine payment: Fine {FineId}, Amount paid: {Amount}",
                    fineId,
                    amount
                );

                var response = await MapToResponseAsync(fine);
                var message =
                    fine.Status == "Paid"
                        ? "Fine paid in full."
                        : $"Partial payment of ${amount} applied. Remaining balance: ${fine.Amount}";

                return ApiResponse<FineResponse>.SuccessResponse(response, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error paying fine: {FineId}", fineId);
                return ApiResponse<FineResponse>.ErrorResponse(
                    "An error occurred while paying fine."
                );
            }
        }

        public async Task<ApiResponse<FineResponse>> WaiveFineAsync(int fineId, string reason)
        {
            try
            {
                var fine = await _fineRepository.GetWithDetailsAsync(fineId);
                if (fine == null)
                {
                    return ApiResponse<FineResponse>.ErrorResponse("Fine not found.");
                }

                if (fine.Status == "Waived" || fine.Status == "Paid")
                {
                    return ApiResponse<FineResponse>.ErrorResponse(
                        $"Fine already {fine.Status.ToLower()}."
                    );
                }

                // Update fine
                fine.Status = "Waived";
                fine.Description += $" Waived. Reason: {reason}";
                fine.UpdatedAt = DateTime.UtcNow;
                fine.IsActive = false;

                // Update member's total fines
                if (fine.Member != null)
                {
                    fine.Member.TotalFines -= fine.Amount;
                    await _memberRepository.UpdateAsync(fine.Member);
                }

                await _fineRepository.UpdateAsync(fine);
                await _fineRepository.SaveChangesAsync();

                _logger.LogInformation(
                    "Fine waived: Fine {FineId}, Reason: {Reason}",
                    fineId,
                    reason
                );

                var response = await MapToResponseAsync(fine);
                return ApiResponse<FineResponse>.SuccessResponse(
                    response,
                    "Fine waived successfully."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error waiving fine: {FineId}", fineId);
                return ApiResponse<FineResponse>.ErrorResponse(
                    "An error occurred while waiving fine."
                );
            }
        }

        public async Task<ApiResponse<decimal>> GetTotalPendingFinesAsync(int memberId)
        {
            try
            {
                var total = await _fineRepository.GetTotalPendingFinesAsync(memberId);
                return ApiResponse<decimal>.SuccessResponse(total);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error calculating pending fines for member: {MemberId}",
                    memberId
                );
                return ApiResponse<decimal>.ErrorResponse(
                    "An error occurred while calculating pending fines."
                );
            }
        }

        public async Task<ApiResponse<bool>> GenerateOverdueFinesAsync()
        {
            try
            {
                var overdueTransactions =
                    await _transactionRepository.GetOverdueTransactionsAsync();
                var generatedCount = 0;

                foreach (var transaction in overdueTransactions)
                {
                    // Check if fine already exists for this transaction
                    var existingFine = await _fineRepository.GetByMemberIdAsync(
                        transaction.MemberId
                    );
                    var hasFine = existingFine.Any(f =>
                        f.TransactionId == transaction.Id && f.Reason == "Overdue" && f.IsActive
                    );

                    if (!hasFine)
                    {
                        var overdueDays = (DateTime.UtcNow - transaction.DueDate).Days;
                        var lateFee = CalculateOverdueFine(overdueDays);

                        if (lateFee > 0)
                        {
                            var fineRequest = new CreateFineRequest
                            {
                                MemberId = transaction.MemberId,
                                TransactionId = transaction.Id,
                                Amount = lateFee,
                                Reason = "Overdue",
                                Description =
                                    $"Overdue fine for book: {transaction.Book?.Title}. Overdue by {overdueDays} days.",
                            };

                            await CreateFineAsync(fineRequest);
                            generatedCount++;
                        }
                    }
                }

                _logger.LogInformation("Generated {Count} overdue fines", generatedCount);
                return ApiResponse<bool>.SuccessResponse(
                    true,
                    $"Generated {generatedCount} overdue fines."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating overdue fines");
                return ApiResponse<bool>.ErrorResponse(
                    "An error occurred while generating overdue fines."
                );
            }
        }

        public async Task<ApiResponse<FineSummaryResponse>> GetFineSummaryAsync(int memberId)
        {
            try
            {
                var member = await _memberRepository.GetWithDetailsAsync(memberId);
                if (member == null)
                {
                    return ApiResponse<FineSummaryResponse>.ErrorResponse("Member not found.");
                }

                var fines = member.Fines;
                var pendingFines = fines.Where(f => f.Status == "Pending" && f.IsActive).ToList();

                var summary = new FineSummaryResponse
                {
                    MemberId = memberId,
                    MemberName =
                        member.User != null
                            ? $"{member.User.FirstName} {member.User.LastName}"
                            : "Unknown",
                    TotalPending = pendingFines.Sum(f => f.Amount),
                    TotalPaid = fines.Where(f => f.Status == "Paid").Sum(f => f.Amount),
                    TotalWaived = fines.Where(f => f.Status == "Waived").Sum(f => f.Amount),
                    OverdueFinesCount = fines.Count(f => f.Reason == "Overdue"),
                    LostBookFinesCount = fines.Count(f => f.Reason == "Lost"),
                    PendingFines = pendingFines
                        .Select(f => new FineSummaryResponse.FineDetail
                        {
                            FineId = f.Id,
                            Amount = f.Amount,
                            Reason = f.Reason,
                            IssueDate = f.IssueDate,
                        })
                        .ToList(),
                };

                return ApiResponse<FineSummaryResponse>.SuccessResponse(summary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting fine summary for member: {MemberId}", memberId);
                return ApiResponse<FineSummaryResponse>.ErrorResponse(
                    "An error occurred while getting fine summary."
                );
            }
        }

        private decimal CalculateOverdueFine(int overdueDays)
        {
            const decimal dailyFee = 0.50m;
            const decimal maxFee = 10.00m;

            var fee = overdueDays * dailyFee;
            return Math.Min(fee, maxFee);
        }

        private async Task<FineResponse> MapToResponseAsync(Fine fine)
        {
            return new FineResponse
            {
                Id = fine.Id,
                MemberId = fine.MemberId,
                MemberName =
                    fine.Member?.User != null
                        ? $"{fine.Member.User.FirstName} {fine.Member.User.LastName}"
                        : "Unknown",
                TransactionId = fine.TransactionId,
                BookTitle = fine.Transaction?.Book?.Title ?? "N/A",
                Amount = fine.Amount,
                Reason = fine.Reason,
                Description = fine.Description,
                IssueDate = fine.IssueDate,
                DueDate = fine.DueDate,
                PaymentDate = fine.PaymentDate,
                Status = fine.Status,
                IsActive = fine.IsActive,
                CreatedAt = fine.CreatedAt,
            };
        }
    }
}
