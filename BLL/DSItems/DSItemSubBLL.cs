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
    public class DSItemSubBLL : BaseBLL, IDSItemSubBLL
    {
        private readonly DSContext _context;
        private readonly IMapper _mapper;

        public DSItemSubBLL(
            DSContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<DSItemSubDto> Add(DSItemSubAddReq req)
        {
            var dsItemEntity = _context.DSItems.FirstOrDefault(x => x.ID == req.DSItemID && x.MemberID == MemberId);
            if (dsItemEntity == null)
            {
                throw new NotFoundException($"DSItem record not found");
            }

            var entity = _mapper.Map<DSItemSub>(req);

            _context.DSItemSubs.Add(entity);

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

            return new DSItemSubDto
            {
                ID = entity.ID,
                DSItemID = entity.DSItemID,
                IsActive = entity.IsActive,
                Name = entity.Name,
            };
        }

        public async Task<DSItemSubDto> Edit(int id, DSItemSubAddReq req)
        {
            var entity = _context.DSItemSubs.FirstOrDefault(x => x.ID == id);
            if (entity == null)
            {
                throw new NotFoundException($"DSItem sub record not found");
            }

            var dsItemEntity = _context.DSItems.FirstOrDefault(x => x.ID == req.DSItemID && x.MemberID == MemberId);
            if (dsItemEntity == null)
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

            return new DSItemSubDto
            {
                ID = entity.ID,
                DSItemID = entity.DSItemID,
                IsActive = entity.IsActive,
                Name = entity.Name,
            };
        }

        public async Task<DSItemSubDto> Delete(int id)
        {
            var entity = _context.DSItemSubs.FirstOrDefault(x => x.ID == id);
            var deletedRecord = entity;

            if (entity == null)
            {
                throw new NotFoundException($"DSItem sub record not found");
            }

            _context.DSItemSubs.Remove(entity);
            _context.SaveChanges();

            return new DSItemSubDto
            {
                ID = deletedRecord.ID,
                DSItemID = deletedRecord.DSItemID,
                IsActive = deletedRecord.IsActive,
                Name = deletedRecord.Name,
            };
        }
    }
}
