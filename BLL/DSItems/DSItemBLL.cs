using AutoMapper;
using demoAPI.BLL.DSItems;
using demoAPI.Common.Helper;
using demoAPI.Data.DS;
using demoAPI.Model;
using demoAPI.Model.DS;
using demoAPI.Model.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace demoAPI.BLL.Member
{
    public class DSItemBLL : BaseBLL, IDSItemBLL
    {
        private readonly DSContext _context;
        private readonly IMapper _mapper;

        public DSItemBLL(
            DSContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DSItemDto>> GetDSItems()
        {
            var dsItems = await _context.DSItems.ToListAsync();

            var dsItemsDto = dsItems.Select(x => new DSItemDto
            {
                ID = x.ID,
                Name = x.Name,
            });

            return dsItemsDto.ToList();
        }

        public async Task<IEnumerable<DSItemWithSubDtoV2>> GetDSItemWithSubV2()
        {
            var responses = (
                from a in _context.DSItems
                join b in _context.DSItemSubs on a.ID equals b.DSItemID into bb
                from b2 in bb.DefaultIfEmpty()
                where a.MemberID == MemberId
                select new
                {
                    ID = a.ID,
                    ItemName = a.Name,
                    DSItemID = b2.DSItemID != null ? b2.DSItemID : 0,
                    SubID = b2.ID != null ? b2.ID : 0,
                    SubName = b2.Name != null ? b2.Name : string.Empty,
                    SubActive = b2.IsActive != null ? b2.IsActive : false,
                }).ToListAsync();

            var dsItems = await responses;

            var resss = new List<DSItemWithSubDtoV2>();

            var dsItemsG = dsItems.GroupBy(x => new { x.ID, x.ItemName });
            foreach (var dsItemG in dsItemsG)
            {
                resss.Add(new DSItemWithSubDtoV2
                {
                    ID = dsItemG.Key.ID,
                    Name = dsItemG.Key.ItemName
                });

                foreach (var dsItem in dsItemG)
                {
                    if (dsItem.SubID == 0) { continue; }

                    resss.Add(new DSItemWithSubDtoV2
                    {
                        ID = dsItemG.Key.ID,
                        Name = dsItemG.Key.ItemName,
                        SubName = dsItem.SubName,
                        SubID = dsItem.SubID,
                    });
                }
            }

            return resss;
        }

        public async Task<IEnumerable<DSItemWithSubDtoV3>> GetDSItemWithSubV3()
        {
            var responses = (
                from a in _context.DSItems
                join b in _context.DSItemSubs on a.ID equals b.DSItemID into bb
                from b2 in bb.DefaultIfEmpty()
                where a.MemberID == MemberId
                select new
                {
                    ID = a.ID,
                    ItemName = a.Name,
                    DSItemID = b2.DSItemID != null ? b2.DSItemID : 0,
                    SubID = b2.ID != null ? b2.ID : 0,
                    SubName = b2.Name != null ? b2.Name : string.Empty,
                    SubActive = b2.IsActive != null ? b2.IsActive : false,
                }).ToListAsync();

            var dsItems = await responses;

            var dsItemsG = dsItems.GroupBy(x => new { x.ID, x.ItemName });
            var ress = new List<DSItemWithSubDtoV3>();

            foreach (var dsItemG in dsItemsG)
            {
                ress.Add(new DSItemWithSubDtoV3
                {
                    Name = dsItemG.Key.ItemName,
                    itemID = dsItemG.Key.ID,
                    itemSubID = 0
                });

                foreach (var dsItem in dsItemG)
                {
                    if (dsItem.SubID == 0) { continue; }

                    ress.Add(new DSItemWithSubDtoV3
                    {
                        Name = $"{dsItemG.Key.ItemName}|{dsItem.SubName}",
                        itemID = 0,
                        itemSubID = dsItem.SubID
                    });
                }
            }

            return ress;
        }

        public async Task<IEnumerable<DSItemWithSubDto>> GetDSItemWithSub()
        {
            var responses = (
                from a in _context.DSItems
                join b in _context.DSItemSubs on a.ID equals b.DSItemID into bb
                from b2 in bb.DefaultIfEmpty()
                where a.MemberID == MemberId
                select new
                {
                    ID = a.ID,
                    ItemName = a.Name,
                    DSItemID = b2.DSItemID != null ? b2.DSItemID : 0,
                    SubID = b2.ID != null ? b2.ID : 0,
                    SubName = b2.Name != null ? b2.Name : string.Empty,
                    SubActive = b2.IsActive != null ? b2.IsActive : false,
                }).ToListAsync();

            var todolist = await responses;

            var resss = new List<DSItemWithSubDto>();

            var fff = todolist.GroupBy(x => new { x.ID, x.ItemName });
            foreach (var item in fff)
            {
                var itemSubs = new List<DSItemSubDto>();

                foreach (var itemSub in item)
                {
                    if (itemSub.SubID == 0) { continue; }

                    itemSubs.Add(new DSItemSubDto
                    {
                        ID = itemSub.SubID,
                        Name = itemSub.SubName,
                        IsActive = itemSub.SubActive,
                        DSItemID = itemSub.DSItemID
                    });
                }

                resss.Add(new DSItemWithSubDto
                {
                    ID = item.Key.ID,
                    Name = item.Key.ItemName,
                    DSItemSubDtos = itemSubs
                });
            }

            return resss;
        }

        public async Task<bool> AddWithSubItem(DSItemAddWithSubItemReq req)
        {
            var dsItemE = new DSItem
            {
                Name = req.Name,
                MemberID = MemberId
            };

            //dsItemE.MemberID = MemberId;

            _context.DSItems.Add(dsItemE);

            try
            {
                _context.SaveChanges();

                if (req.SubName.IsNotNullOrEmpty())
                {
                    //add sub item
                    var dsItemSubE = new DSItemSub
                    {
                        DSItemID = dsItemE.ID,
                        Name = req.SubName
                    };

                    _context.DSItemSubs.Add(dsItemSubE);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                if (ex is DbUpdateException)
                {
                    throw new BadRequestException(ex.InnerException?.Message);
                }
            }

            return true;
        }

        public async Task<DSItem> Add(DSItemAddReq req)
        {
            var entity = _mapper.Map<DSItem>(req);
            entity.MemberID = MemberId;

            _context.DSItems.Add(entity);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex is DbUpdateException)
                {
                    throw new BadRequestException(ex.InnerException?.Message);
                }
            }

            return entity;
        }

        public async Task<DSItem> Edit(int id, DSItemAddReq req)
        {
            var entity = _context.DSItems.FirstOrDefault(x => x.ID == id && x.MemberID == MemberId);
            if (entity == null)
            {
                throw new NotFoundException($"DSItem record not found");
            }

            _mapper.Map(req, entity);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex is DbUpdateException)
                {
                    throw new BadRequestException(ex.InnerException?.Message);
                }
            }

            return entity;
        }

        public async Task<DSItem> Delete(int id)
        {
            var entity = _context.DSItems.FirstOrDefault(x => x.ID == id && x.MemberID == MemberId);
            var deletedRecord = entity;

            if (entity == null)
            {
                throw new NotFoundException($"DSItem record not found");
            }

            _context.DSItems.Remove(entity);
            _context.SaveChanges();

            return deletedRecord;
        }
    }
}
