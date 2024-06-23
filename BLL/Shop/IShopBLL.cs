using demoAPI.Model;
using demoAPI.Model.DS.Shops;

namespace demoAPI.BLL.Shop
{
    public interface IShopBLL
    {
        Task<List<ShopDiaryDto>> GetShopDiaries();
        Task<List<ShopDiaryDto>> GetShopDiaries(int shopId);
        Task<ShopDiaryDto> AddShopDiary(ShopDiaryAddReq req);
        Task<ShopDiaryDto> UpdateShopDiary(int id, ShopDiaryAddReq req);
        Task<ShopDiaryDto> DeleteShopDiary(int id);

        Task<List<ShopDto>> GetShops();
        Task<ShopDto> AddShop(ShopAddReq req);
        Task<ShopDto> UpdateShop(int id, ShopAddReq req);
        Task<ShopDto> DeleteShop(int id);

        Task<List<ShopTypeDto>> GetShopTypes();
        Task<ShopTypeDto> AddShopType(ShopTypeAddReq req);
        Task<ShopTypeDto> UpdateShopType(int id, ShopTypeAddReq req);
        Task<ShopTypeDto> DeleteShopType(int id);
    }
}
