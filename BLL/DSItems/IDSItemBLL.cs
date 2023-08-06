using demoAPI.Model.DS;

namespace demoAPI.BLL.DSItems
{
    public interface IDSItemBLL
    {
        Task<IEnumerable<DSItemDto>> GetDSItems();
        Task<IEnumerable<DSItemWithSubDtoV3>> GetDSItemWithSubV3();
        Task<IEnumerable<DSItemWithSubDtoV2>> GetDSItemWithSubV2();
        Task<IEnumerable<DSItemWithSubDto>> GetDSItemWithSub();

        Task<DSItem> Add(DSItemAddReq req);
        Task<bool> AddWithSubItem(DSItemAddWithSubItemReq req);

        Task<DSItem> Edit(int id, DSItemAddReq req);

        Task<DSItem> Delete(int id);
    }
}
