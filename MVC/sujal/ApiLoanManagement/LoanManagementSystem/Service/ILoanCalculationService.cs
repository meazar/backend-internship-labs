using LoanManagementSystem.Models;

namespace LoanManagementSystem.Service
{
    public interface ILoanCalculationService
    {

        (decimal emi, decimal totalInterest, decimal totalPayable) CalculateEMI(decimal principal, decimal annualInterestRate, int tenureMonths);

        IEnumerable<PaymentSchedule> GeneratePaymentSchedule(int loanAccountId, decimal principle,decimal annualInterestRate, int tenureMonths, DateTime startDate);

    }
}
