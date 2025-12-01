using LoanManagementSystem.IRepository;
using LoanManagementSystem.Models;
using LoanManagementSystem.Repository;
using LoanManagementSystem.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Linq.Expressions;

namespace LoanManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    [Authorize]
    public class LoanController : ControllerBase
    {

        private readonly ILoanRepository _loanRepository;
        private readonly ILoanCalculationService _loanCalculationService;
        private readonly ILogger<LoanController> _logger;

        public LoanController(ILoanRepository loanRepository, ILoanCalculationService loanCalculationService, ILogger<LoanController> logger)
        {
            _loanRepository = loanRepository;
            _loanCalculationService = loanCalculationService;
            _logger = logger;
        }



        [HttpGet("/loan-types")]
        
        public async Task<IActionResult> GetLoanType()
        {
            try
            {
                var loanType = await _loanRepository.GetAllLoanTypesAsync();
                return Ok(loanType);


            }catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR IN GETTING LOAN TYPE...");
                return StatusCode(500, new { message = "Internal Server error" });
            }
        }

        [HttpPost("/create")]

        [Authorize(Roles = "Customer")]

        public async Task<IActionResult> CreateLoanApplication([FromBody] CreateLoanApplication request)
        {
            try
            {
                var loanType = await _loanRepository.GetLoanTypeByIdAsync(request.LoanTypeId);
                if (loanType == null)
                {
                    return BadRequest(new { message = "Invalid loan type" });
                }

                if (request.RequestedAmount < loanType.MinAmount || request.RequestedAmount > loanType.MaxAmount)
                {
                    return BadRequest(new
                    {
                        message = $"Requested amount must be between {loanType.MinAmount} and {loanType.MaxAmount}"

                    });

                }
                if (request.DurationMonths > loanType.MaxDurationMonths)
                {
                    return BadRequest(new
                    {
                        message = $"Loan duration cannot exceed {loanType.MaxDurationMonths} months"
                    });

                }
                var applicationId = await _loanRepository.CreateLoanApplicationAsync(request);
                if (applicationId > 0)
                {
                    return Ok(new { message = "Loan Application Submitted Successfully", applicationId });

                }
                return StatusCode(500, new { message = "Failed to Create Loan Application" });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR CREATEING LOAN APPLICATION");
                return StatusCode(500, new { message = ex.Message });
            }
        }




        [HttpGet("applications/user/{userId}")]


        public async Task<IActionResult> GetUserApplication(int userId)
        {
            try
            {
                var applications = await _loanRepository.GetLoanApplicationByUserIdAsync(userId);
                return Ok(applications);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in getting user application");
                return StatusCode(500, new {message = ex.Message});

            }
        }

        
        [HttpGet("application/pending")]
        [Authorize(Roles = "Admin,Officer")]
        public async Task<IActionResult> GetPendingApplication()
        {
            try
            {
                var applications = await _loanRepository.GetPendingApplicationAsync();
                return Ok(applications);

            }catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting pending application");
                return StatusCode(500, new {message = ex.Message});

            }
        }

        [HttpPost("applications/{applicationId}/approve")]
        [Authorize(Roles = "Admin,Officer")]


        public async Task<IActionResult> ApproveLoanApplication(int applicationId, [FromBody] ApproveLoanRequest request)
        {
            try
            {
                var application = await _loanRepository.GetLoanApplicationByIdAsync(applicationId);
                if(application == null)
                {
                    return NotFound(new { message = "Loan application not found" });
                }


                await _loanRepository.UpdateLoanApplicationStatusAsync(
                    applicationId, "Approved", request.VerifiedBy, request.Remarks);

                var loanType = await _loanRepository.GetLoanTypeByIdAsync(application.LoanTypeId);
                var (emi, totalInterest, totalPayable) = _loanCalculationService.CalculateEMI(
                    application.RequestedAmount, loanType!.InterestRate, application.DurationMonth);

                var loanAccount = new LoanAccount
                {
                    ApplicationId = applicationId,
                    UserId = application.UserId,
                    PrincipalAmount = application.RequestedAmount,
                    InterestRate = loanType.InterestRate,
                    EMIAmount = emi,
                    TotalInterest = totalInterest,
                    TotalPayable = totalPayable,
                    BalanceAmount = totalPayable,
                    PenaltyAmount = 0,
                    LoanStatus = "Active",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddMonths(application.DurationMonth),

                };
                var loanAccountId = await _loanRepository.CreateLoanAccountAsync(loanAccount);

                return Ok(new
                {
                    message = "Loan Application approved successfully",
                    loanAccountId
                });

            }catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving loan application");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }


        [HttpPost("applications/{applicationId}/reject")]
        [Authorize(Roles = "Admin,Officer")]


        public async Task<IActionResult> RejectLoanAppplication(int applicationId, [FromBody] RejectLoanRequest request)
        {
            try
            {
                var application = await _loanRepository.GetLoanApplicationByIdAsync(applicationId);
                if (application == null)
                {
                    return NotFound(new { message = "Loan application not found" });
                }

                await _loanRepository.UpdateLoanApplicationStatusAsync(
                    applicationId, "Rejected", request.VerifiedBy, request.Remarks);
                return Ok(new { message = "Loan application rejected successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rejecting  loan application");

                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpGet("account/user/{userId}")]
        [Authorize(Roles = "Admin,Officer")]
        public async Task<IActionResult> GetUserLoanAccounts(int userId)
        {
            try
            {
                var accounts = await _loanRepository.GetLoanAccountByUserIdAsync(userId);
                return Ok(accounts);
            } catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR GETTING USER LOAN ACCOUNTS");
                return StatusCode(500, new { message = ex.Message });
            }

        }

        public class ApproveLoanRequest
        {
            public int VerifiedBy { get; set; }
            public string Remarks { get; set; } = string.Empty;
        }

        public class RejectLoanRequest
        {
            public int VerifiedBy { get; set; }
            public string Remarks { get; set; } = string.Empty;
        }


    }
}
