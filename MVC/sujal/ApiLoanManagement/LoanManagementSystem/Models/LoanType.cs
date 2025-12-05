namespace LoanManagementSystem.Models
{
    public class LoanType
    {
        public int LoanTypeId {get; set;}
        public string LoanName {get; set;}

        public decimal InterestRate {get; set;}

        public decimal ProcessingFee {get; set;}

        public decimal MaxAmount {get; set;}

        public decimal MinAmount {get; set;}

        public int MaxDurationMonths {get; set;}






    }
}
