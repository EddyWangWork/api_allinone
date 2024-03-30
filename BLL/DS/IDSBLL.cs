using demoAPI.Model.DS;

namespace demoAPI.BLL.DS
{
    public interface IDSBLL
    {
        #region Dashboard

        Task<IEnumerable<DSMonthlyItemExpenses>> GetDSMonthlyItemExpensesAsync(int year, int month, int monthDuration);
        Task<IEnumerable<DSMonthlyPeriodCreditDebit>> GetDSMonthlyPeriodCreditDebitAsync(int year, int month, int monthDuration);
        Task<IEnumerable<DSMonthlyPeriodCreditDebit>> GetDSMonthlyPeriodCreditDebitAsyncV2(int year, int month, int monthDuration,
            bool isIncludeCredit, List<int> creditIds, bool isIncludeDebit, List<int> debitIds);
        Task<DSMonthlyExpenses> GetDSMonthlyCommitmentAndOtherAsync(int year, int month, string name);
        Task<IEnumerable<DSMonthlyExpensesItem>> GetDSMonthlyExpensesAsync(int year, int month, string name, bool isExclude = false);

        #endregion

        Task<DSYearExpenses> GetDSYearExpensesAsync(int year);
        Task<IEnumerable<DSYearCreditDebitDiff>> GetDSYearCreditDebitDiffAsync(int year);
        Task<IEnumerable<DSDebitStat>> GetDSMonthlyExpensesAsync(int year, int month);
        Task<IEnumerable<DSTransactionDto>> GetDSTransactionAsync();
        Task<IEnumerable<DSTransactionDtoV2>> GetDSTransactionAsyncV2(DateTime? dateFrom = null, DateTime? dateTo = null);
        Task<DSTransactionDto> Add(DSTransactionReq req);
        Task<bool> Edit(int id, DSTransactionReq req);
        Task<bool> Delete(int id);
    }
}
