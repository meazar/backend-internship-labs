using LoanManagementSystem.Models;

namespace LoanManagementSystem.IRepository
{
    public interface ILoanRepository
    {
        Task <IEnumerable<LoanType>> GetAllLoanTypesAsync ();
        Task<LoanType> GetLoanTypeByIdAsync(int loadTypeId);
        Task<int> CreateLoanApplicationAsync (CreateLoanApplication application);

        Task<LoanApplication> GetLoanApplicationByIdAsync (int applicationid);

        Task<IEnumerable<LoanApplication>> GetLoanApplicationByUserIdAsync (int userId);

        Task<IEnumerable<LoanApplication>> GetPendingApplicationAsync ();
        Task<bool> UpdateLoanApplicationStatusAsync(int applicationId, string status, int verifiedBy, string remarks);

        Task<int> CreateLoanAccountAsync(LoanAccount account);
        Task <LoanAccount?> GetLoanAccountByIdAsync(int loanAccountid);

        Task<IEnumerable<LoanAccount>> GetLoanAccountByUserIdAsync (int userId);

        Task<bool> UpdateLoanAccountBalanceAsync(int loanAccountId, decimal newBalance);



    }
}
