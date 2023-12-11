using demoAPI.Model.DS;

namespace demoAPI.BLL.DS
{
    public interface IDSBLL
    {
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
