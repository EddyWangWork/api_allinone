using AutoMapper;
using demoAPI.Data.DS;
using demoAPI.Model.DS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace demoAPI.BLL.DS
{
    public class DSBLL : BaseBLL, IDSBLL
    {
        private readonly DSContext _context;
        private readonly IMapper _mapper;

        private readonly List<int> _transferTypes = new() { 3, 4 };

        public DSBLL(DSContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DSTransactionDto>> GetDSTransactionAsync()
        {
            var finalRes = new List<DSTransactionDto>();

            var responses = (
                 from a in _context.DSTransactions
                 join b in _context.DSAccounts on a.DSAccountID equals b.ID
                 join c in _context.DSTypes on a.DSTypeID equals c.ID
                 join d in _context.DSItems on a.DSItemID equals d.ID into dd
                 from d2 in dd.DefaultIfEmpty()
                 join e in _context.DSItemSubs on a.DSItemSubID equals e.ID into ee
                 from e2 in ee.DefaultIfEmpty()
                 join f in _context.DSItems on e2.DSItemID equals f.ID into ff
                 from f2 in ff.DefaultIfEmpty()
                 join g in _context.DSTransactions on a.DSTransferOutID equals g.ID into gg
                 from g2 in gg.DefaultIfEmpty()
                 join h in _context.DSAccounts on g2.DSAccountID equals h.ID into hh
                 from h2 in hh.DefaultIfEmpty()
                 where a.MemberID == MemberId
                 select new DSTransactionDto
                 {
                     DSTypeName = c.Name,
                     DSAccountName = b.Name,
                     DSItemName = c.ID == 4 ? h2.Name : d2.ID > 0 ? d2.Name : $"{f2.Name}|{e2.Name}",
                     ID = a.ID,
                     DSTypeID = a.DSTypeID,
                     DSAccountID = a.DSAccountID,
                     DSTransferOutID = a.DSTransferOutID,
                     DSItemID = a.DSItemID,
                     DSItemSubID = a.DSItemSubID,
                     Description = a.Description,
                     Amount = a.Amount,
                 }).ToListAsync();

            var res2 = await responses;

            foreach (var item in res2)
            {
                if (item.DSTypeID == 3)
                {
                    item.DSItemName = res2.FirstOrDefault(x => x.DSTransferOutID == item.ID).DSAccountName;
                }
            }

            return res2;
        }

        public async Task<DSTransactionDto> Add(DSTransactionReq req)
        {
            var entity = _mapper.Map<DSTransaction>(req);

            _context.DSTransactions.Add(entity);
            _context.SaveChanges();

            if (req.DSTypeID == 3)
            {
                DSTransaction entityToAccount = new DSTransaction
                {
                    DSTypeID = 4,
                    Amount = entity.Amount,
                    Description = entity.Description,
                    DSAccountID = req.DSAccountToID,
                    DSTransferOutID = entity.ID
                };
                _context.DSTransactions.Add(entityToAccount);
                _context.SaveChanges();
            }

            return new DSTransactionDto();
        }
    }
}
