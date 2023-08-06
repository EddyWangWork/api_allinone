using demoAPI.Model.DS;

namespace demoAPI.BLL
{
    public interface IDSAccountBLL
    {
        Task<List<DSAccountDto>> GetDSAccounts();
    }
}
