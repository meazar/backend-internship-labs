using LoanManagementSystem.Models;

namespace LoanManagementSystem.Service
{
    public class LoanCalculationService : ILoanCalculationService
    {
        
        public (decimal emi, decimal totalInterest, decimal totalPayable) CalculateEMI(decimal principal, decimal annualInterestRate, int tenureMonths)
        {
            var monthlyRate = (double)(annualInterestRate /100/12);
            var principalAmount = (double)principal;
            var tenure = tenureMonths;


            var emi = principalAmount * monthlyRate * Math.Pow(1 + monthlyRate, tenure) / (Math.Pow(1 + monthlyRate, tenure) - 1);

            var totalPayable = emi * tenure;

            var totalInterest = totalPayable - principalAmount;

            return ((decimal)emi, (decimal)totalInterest, (decimal)totalPayable);


        }

        public IEnumerable<PaymentSchedule> GeneratePaymentSchedule(int loanAccountId,decimal principal, decimal annualInterestRate,int tenureMonths,DateTime startDate) 
        {
            var schedule = new List<PaymentSchedule>();
            var monthlyRate  = (double)(annualInterestRate /100/12);
            var balance = (double)principal;

            var (emi, totalInterest, totalPayable) = CalculateEMI(principal,annualInterestRate,tenureMonths);

            var monthlyEMI = (double)emi;

            for(int i =1;i<=loanAccountId;i++)
            {
                var interestPortion = balance * monthlyRate;
                var principalPortion = monthlyEMI - interestPortion;

                if(i ==  tenureMonths)
                {
                    principalPortion = balance;
                    monthlyEMI = principalPortion + interestPortion;


                }

                balance -= principalPortion;

                schedule.Add(new PaymentSchedule
                {
                    LoanAccountId = loanAccountId,
                    InstallationNO = i,
                    DueDate = startDate.AddMonths(i),
                    Amount = (decimal) monthlyEMI,
                    PrincipalPortion = (decimal)principalPortion,
                    InterestPortion = (decimal)interestPortion,
                    Status = "Pending",
                    PenaltyAdded = 500,
                });

            }

            return schedule;

        }
    }
}
