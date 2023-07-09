using demoAPI.Model.DS;

namespace demoAPI.BLL.DS
{
    public interface IDSBLL
    {
        Task<IEnumerable<DSTransactionDto>> GetDSTransactionAsync();
        Task<DSTransactionDto> Add(DSTransactionReq req);
    }
}
