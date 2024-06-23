using AutoMapper;
using AutoMapper.Execution;
using demoAPI.BLL.Kanbans;
using demoAPI.Common.Enum;
using demoAPI.Common.Helper;
using demoAPI.Data.DS;
using demoAPI.Model;
using demoAPI.Model.DS.Shops;
using demoAPI.Model.Exceptions;
using demoAPI.Model.Kanbans;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using static Azure.Core.HttpHeader;

namespace demoAPI.BLL.Shop
{
    public class ShopBLL : BaseBLL, IShopBLL
    {
        private readonly DSContext _context;
        private readonly IMapper _mapper;

        public ShopBLL(DSContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        #region Shop Diary

        public async Task<List<ShopDiaryDto>> GetShopDiaries()
        {
            try
            {
                var shops = await GetShopsData();
                var shopIds = shops.Select(x => x.ID);

                var shopDiaries = await _context.ShopDiaries.Where(x => shopIds.Contains(x.ShopID)).ToListAsync();

                var response = shopDiaries.Select(x =>
                {
                    var shop = shops.FirstOrDefault(y=>y.ID == x.ShopID);

                    return ToShopDiaryDto(x, shop);
                });

                return response.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<ShopDiaryDto>> GetShopDiaries(int shopId)
        {
            try
            {
                var shop = await _context.Shops.FirstOrDefaultAsync(x => x.ID == shopId) ?? throw new NotFoundException($"Shop record not found");
                var shopDiaries = await _context.ShopDiaries.Where(x => x.ShopID == shopId).ToListAsync() ?? throw new NotFoundException($"ShopDiary record not found");

                var response = shopDiaries.Select(x =>
                {                    
                    return ToShopDiaryDto(x, shop);
                });

                return response.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ShopDiaryDto> GetShopDiary(int id)
        {
            try
            {
                var shops = await GetShopsData();
                var shopIds = shops.Select(x => x.ID);

                var shopDiary = await _context.ShopDiaries.FirstOrDefaultAsync(x => x.ID == id) ?? throw new NotFoundException($"ShopDiary record not found");

                var shop = shops.FirstOrDefault(x => x.ID == shopDiary.ShopID) ?? throw new NotFoundException($"Shop record not found");

                var response = ToShopDiaryDto(shopDiary, shop);

                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ShopDiaryDto> AddShopDiary(ShopDiaryAddReq req)
        {
            var entity = _mapper.Map<ShopDiary>(req);

            _context.ShopDiaries.Add(entity);
            _context.SaveChanges();

            return await GetShopDiary(entity.ID);
        }

        public async Task<ShopDiaryDto> UpdateShopDiary(int id, ShopDiaryAddReq req)
        {
            var entity = _context.ShopDiaries.FirstOrDefault(x => x.ID == id) ?? throw new NotFoundException($"ShopDiary record not found");

            _mapper.Map(req, entity);
            _context.SaveChanges();

            return await GetShopDiary(id);
        }

        public async Task<ShopDiaryDto> DeleteShopDiary(int id)
        {
            var entity = _context.ShopDiaries.FirstOrDefault(x => x.ID == id) ?? throw new NotFoundException($"ShopDiary record not found");
            var deletedRecord = await GetShopDiary(id); ;

            _context.ShopDiaries.Remove(entity);
            _context.SaveChanges();

            return deletedRecord;
        }

        #endregion

        #region Shop

        public async Task<List<ShopDto>> GetShops()
        {
            try
            {
                var shops = await GetShopsData();

                var shopDiaries = await _context.ShopDiaries.ToListAsync();

                shops.ForEach(x =>
                {                    
                    x.VisitedCount = shopDiaries.Where(y => y.ShopID == x.ID).Count();
                });

                return shops;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        public async Task ProcessUpdateShopsTypes()
        {
            try
            {
                var responses = await _context.Shops.Where(x => x.MemberID == MemberId).ToListAsync();
                var shopTypes = await GetShopTypes();

                responses.ForEach(x =>
                {
                    var intTypes = GetTypesToIntList(x.Types);
                    x.Types = GetTypesToString(intTypes, shopTypes);
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ShopDto> AddShop(ShopAddReq req)
        {
            var entity = _mapper.Map<Model.Shop>(req);
            entity.MemberID = MemberId;

            var shopTypes = await GetShopTypes();

            entity.Types = GetTypesToString(req.TypeList, shopTypes);

            _context.Shops.Add(entity);
            _context.SaveChanges();

            return ToShopDto(entity, shopTypes);
        }

        public async Task<ShopDto> UpdateShop(int id, ShopAddReq req)
        {
            var entity = _context.Shops.FirstOrDefault(x => x.ID == id) ?? throw new NotFoundException($"Shop record not found");

            var shopTypes = await GetShopTypes();

            entity.Types = GetTypesToString(req.TypeList, shopTypes);

            _mapper.Map(req, entity);
            _context.SaveChanges();

            return ToShopDto(entity, shopTypes);
        }

        public async Task<ShopDto> DeleteShop(int id)
        {
            var entity = _context.Shops.FirstOrDefault(x => x.ID == id) ?? throw new NotFoundException($"Shop record not found");
            var deletedRecord = entity;

            _context.Shops.Remove(entity);
            _context.SaveChanges();

            var shopTypes = await GetShopTypes();

            return ToShopDto(entity, shopTypes);
        }

        #endregion

        #region Shop Type

        public async Task<List<ShopTypeDto>> GetShopTypes()
        {
            try
            {
                var responses = await _context.ShopTypes.ToListAsync();
                return responses.Where(x => x.MemberID == MemberId).Select(x => _mapper.Map<ShopTypeDto>(x)).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ShopTypeDto> AddShopType(ShopTypeAddReq req)
        {
            var entity = _mapper.Map<ShopType>(req);
            entity.MemberID = MemberId;

            _context.ShopTypes.Add(entity);
            _context.SaveChanges();

            return _mapper.Map<ShopTypeDto>(entity);
        }

        public async Task<ShopTypeDto> UpdateShopType(int id, ShopTypeAddReq req)
        {
            var entity = _context.ShopTypes.FirstOrDefault(x => x.ID == id) ?? throw new NotFoundException($"ShopType record not found");

            _mapper.Map(req, entity);
            _context.SaveChanges();

            return _mapper.Map<ShopTypeDto>(entity);
        }

        public async Task<ShopTypeDto> DeleteShopType(int id)
        {
            var entity = _context.ShopTypes.FirstOrDefault(x => x.ID == id) ?? throw new NotFoundException($"ShopType record not found");
            var deletedRecord = entity;

            _context.ShopTypes.Remove(entity);
            _context.SaveChanges();

            await ProcessUpdateShopsTypes();

            return _mapper.Map<ShopTypeDto>(deletedRecord);
        }

        #endregion

        #region Private

        private async Task<List<ShopDto>> GetShopsData()
        {
            try
            {
                var responses = await _context.Shops.ToListAsync();

                var shopTypes = await GetShopTypes();

                var shopDtos = responses.Where(x => x.MemberID == MemberId).Select(x => ToShopDto(x, shopTypes));

                return shopDtos.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private ShopDto ToShopDto(Model.Shop entity, List<ShopTypeDto> shopTypes)
        {
            var dto = _mapper.Map<ShopDto>(entity);

            if (entity.Types.IsNullOrEmpty())
            {
                dto.TypeList = new List<string>();
                return dto;
            }

            var typeIntList = GetTypesToIntList(entity.Types);
            var typeNameList = shopTypes.Where(x => typeIntList.Contains(x.ID)).Select(x => x.Name);
            dto.TypeList = typeNameList.ToList();

            return dto;
        }


        private ShopDiaryDto ToShopDiaryDto(ShopDiary entity, ShopDto shopDto)
        {
            return new ShopDiaryDto
            {
                ID = entity.ID,
                Comment = entity.Comment,
                Date = entity.Date,
                Remark = entity.Remark,
                ShopID = shopDto.ID,
                ShopName = shopDto.Name
            };
        }

        private ShopDiaryDto ToShopDiaryDto(ShopDiary entity, Model.Shop shopDto)
        {
            return new ShopDiaryDto
            {
                ID = entity.ID,
                Comment = entity.Comment,
                Date = entity.Date,
                Remark = entity.Remark,
                ShopID = shopDto.ID,
                ShopName = shopDto.Name
            };
        }

        private static List<int> GetTypesToIntList(string types)
        {
            var response = types.Split(',').Select(x =>
            {
                return Convert.ToInt32(x);
            });

            return response.ToList();
        }

        private static string GetTypesToString(List<int> types, List<ShopTypeDto> shopTypes)
        {
            var intTypes = types.GroupBy(p => p).Select(g => g.First()).ToList();

            if (!intTypes.Any())
                return "";

            var validTypeIds = shopTypes.Where(x => types.Contains(x.ID)).Select(x => x.ID);

            var response = validTypeIds.Count() > 1 ? String.Join(",", validTypeIds) : validTypeIds.FirstOrDefault().ToString();

            return response;
        }

        #endregion
    }
}
