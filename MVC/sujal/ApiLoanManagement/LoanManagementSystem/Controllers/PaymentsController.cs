using LoanManagementSystem.IRepository;
using LoanManagementSystem.Models;
using LoanManagementSystem.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LoanManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : Controller
    {

        private readonly IPaymentRepository _paymentRepository;
        private readonly ILogger <PaymentsController>_logger;
        private readonly ILoanRepository _loanRepository; 
        private readonly IAuditRepository _auditRepository;
        private readonly ILoanCalculationService _loanCalculationService;


        

        public PaymentsController(IPaymentRepository paymentRepository, ILogger<PaymentsController> logger, ILoanRepository loanRepository, IAuditRepository auditRepository, ILoanCalculationService loanCalculationService)
        {
            _paymentRepository = paymentRepository;
            _logger = logger;
            _loanRepository = loanRepository;
            _auditRepository = auditRepository;
            _loanCalculationService = loanCalculationService;
        }


        [HttpPost("make-payment")]
        [Authorize(Roles = "Admin,Officer")]

        public async Task<IActionResult> MakePayment([FromBody] MakePaymentRequest request)
        {
            try
            {
                var schedule = await _paymentRepository.GetPaymentScheduleByIdAsync(request.ScheduleId);

                if(schedule == null)
                {
                    return NotFound(new { message = "Payment schedule not found" });
                }

                if(schedule.Status == "paid")
                {
                    return BadRequest(new { message = "THis  installment is already paid" });

                }

                var loanAccount = await _loanRepository.GetLoanAccountByIdAsync(request.LoanAccountId);

                if(loanAccount == null)
                {
                    return BadRequest(new { message = "Loan Account Not FOund" });

                }

                var totalDue = schedule.Amount + schedule.PenaltyAdded;

                if (request.AmoundPaid < totalDue)
                {
                    return BadRequest(new { messsage = $"payment amount must be at least{totalDue}" });

                }

                var payment = new Payments
                {
                    LoanAccountId = request.LoanAccountId,
                    ScheduleId = request.ScheduleId,
                    AmountPaid = request.AmoundPaid,
                    PaymentMethod = request.PaymentMethod,
                    TransactionId = request.TransactionId,
                    Remarks = request.Remarks,
                    PaymentDate = DateTime.Now,
                };
                var paymentId = await _paymentRepository.MakePaymentAsync(payment);
                if(paymentId == 0)
                {
                    return StatusCode(500, new { message = " Failded to process  payment " });

                }

                await _paymentRepository.UpdatePaymentScheduleStatusAsync(request.ScheduleId, "paid", DateTime.Now);

                var newBalance = loanAccount.BalanceAmount - schedule.Amount;
                await _loanRepository.UpdateLoanAccountBalanceAsync(request.LoanAccountId, newBalance);



                await _auditRepository.LogActionAsync(new AuditLog
                {
                    UserId = loanAccount.UserId,
                    Action = "PAYMENT MADE",
                    TableName = "Payments",
                    NewValue = $"Payment of {request.AmoundPaid} for schedule {request.ScheduleId}",


                });

                return Ok(new
                {
                    message = "Payment processed Successfully",
                    paymentId,
                    newBalance

                });


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error gettting payment schedule");
                return StatusCode(500, new { message = "Internal server error" });


            }
        }

        [HttpGet("schedule/{loanAccountId}")]

        public async Task<IActionResult> GetPaymentSchedule(int loanAccountId)
        {
            try
            {
                var schedule = await _paymentRepository.GetPaymentScheduleByLoanAccountAsync(loanAccountId);
                return Ok(schedule);

            }catch(Exception ex)
            {
                _logger.LogError(ex, "Error getting payment schedule");
                return StatusCode(500, new { message = "Internal server error" });

            }
        }



        [HttpGet("history/{loanAccountId}")]
        public async Task<IActionResult> GetPaymentHistory(int loanAccountId)
        {
            try
            {
                var payments = await _paymentRepository.GetPaymentsByLoanAccountAsync(loanAccountId);
                return Ok(payments);

            }catch(Exception ex)
            {
                _logger.LogError(ex, "Error getting payment history");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }


        [HttpPost("generate-schedule/{loanAccountId}")]
        [Authorize(Roles = "Admin,Officer")]
        public async Task<IActionResult> GeneratePaymentSchedule(int loanAccountId)
        {
            try
            {
                var loanAccount = await _loanRepository.GetLoanAccountByIdAsync(loanAccountId);
                if (loanAccount == null)
                {
                    return NotFound(new { message = "Loan account not found" });

                }

                var schedule = _loanCalculationService.GeneratePaymentSchedule(
                    loanAccountId,
                    loanAccount.PrincipalAmount,
                    loanAccount.InterestRate,
                    (int)(loanAccount.EndDate - loanAccount.StartDate).TotalDays / 30,
                    loanAccount.StartDate
                    );
                foreach (var intallation in schedule)
                {
                    await _paymentRepository.CreatePaymentScheduleAsync(intallation);

                }
                return Ok(new
                {
                    message = "PAYMENT schedule generated successfully",
                    installation = schedule.Count()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error generating payment schedule");
                return StatusCode(500, new { message = "Internal server error" });
            }
            
        }

        [HttpPost("add-penalty")]
        [Authorize (Roles = "Admin,Officer")]
        public async Task<IActionResult> AddPenalty([FromBody] AddPenaltyRequest request)
        {
            try
            {
                var penalty = new Penalty
                {
                    LoanAccountId = request.LoanAccountId,
                    ScheduleId = request.ScheduleId,
                    PenaltyAmount = request.PenaltyAmount,
                    Status = "Unpaid",
                    CreatedAt = DateTime.UtcNow,

                };
                var success = await _paymentRepository.AddPenaltyAsync(penalty);
                if (!success)
                {
                    return StatusCode(500, new { message = "Failed to add penalty" });

                }
                var schedule = await _paymentRepository.GetPaymentScheduleByIdAsync(request.ScheduleId);
                if (schedule != null)
                {

                }
                return Ok(new { message = "Penalty add successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error adding penalty");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }


       
    }

    
}
