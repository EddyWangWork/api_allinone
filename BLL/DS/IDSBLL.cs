using demoAPI.Model.DS;

namespace demoAPI.BLL.DS
{
    public interface IDSBLL
    {
        Task<IEnumerable<DSTransactionDto>> GetDSTransactionAsync();
        Task<IEnumerable<DSTransactionDtoV2>> GetDSTransactionAsyncV2();
        Task<DSTransactionDto> Add(DSTransactionReq req);
        Task<bool> Edit(int id, DSTransactionReq req);
        Task<bool> Delete(int id);
    }
}
