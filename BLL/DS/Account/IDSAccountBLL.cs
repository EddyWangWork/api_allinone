using demoAPI.Model.DS;
using demoAPI.Model.DS.Accounts;

namespace demoAPI.BLL
{
    public interface IDSAccountBLL
    {
        Task<List<DSAccountDto>> GetDSAccounts();
        Task<List<DSAccountDto>> GetDSAccounts(bool isActive);
        Task<List<DSAccountDto>> GetDSAccountsWithBalance();
        Task<DSAccount> Add(DSAccountAddReq req);
        Task<DSAccount> Edit(int id, DSAccountAddReq req);
        Task<DSAccount> Delete(int id);
    }
}
