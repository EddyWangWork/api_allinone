using demoAPI.Model.DS;

namespace demoAPI.BLL.DSItems
{
    public interface IDSItemSubBLL
    {
        Task<DSItemSubDto> Add(DSItemSubAddReq req);
        Task<DSItemSubDto> Edit(int id, DSItemSubAddReq req);
        Task<DSItemSubDto> Delete(int id);
    }
}
