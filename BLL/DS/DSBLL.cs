using AutoMapper;
using demoAPI.Common.Enum;
using demoAPI.Data.DS;
using demoAPI.Model.DS;
using demoAPI.Model.Exceptions;
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

        public async Task<IEnumerable<DSTransactionDtoV2>> GetDSTransactionAsyncV2(DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            var finalRes = new List<DSTransactionDtoV2>();

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
                     CreatedDateTime = a.CreatedDateTime,
                     Amount = a.Amount,
                 }).ToListAsync();

            var dsTransactions = await responses;

            var dsaccountids = dsTransactions.DistinctBy(x => x.DSAccountID).Select(x => x.DSAccountID);
            List<int> expensesList =
                new List<int> { (int)EnumDSTranType.Expense, (int)EnumDSTranType.TransferOut, (int)EnumDSTranType.DebitTransferOut };
            int rowID = 0;

            foreach (var dsaccountid in dsaccountids)
            {
                decimal balance = 0;
                var dsTransactionsByAcc = dsTransactions.Where(x => x.DSAccountID == dsaccountid).OrderBy(x => x.CreatedDateTime);

                foreach (var dsTransactionByAcc in dsTransactionsByAcc)
                {
                    var dsTransferOutTran = new DSTransactionDto();
                    if (dsTransactionByAcc.DSTypeID == 3)
                    {
                        dsTransferOutTran = dsTransactions.FirstOrDefault(x => x.DSTransferOutID == dsTransactionByAcc.ID);
                    }

                    balance = expensesList.Contains(dsTransactionByAcc.DSTypeID) ? balance - dsTransactionByAcc.Amount : balance + dsTransactionByAcc.Amount;
                    finalRes.Add(new DSTransactionDtoV2
                    {
                        RowID = rowID++,
                        DSTypeName = dsTransactionByAcc.DSTypeName,
                        DSAccountName = dsTransactionByAcc.DSAccountName,
                        DSItemName = dsTransactionByAcc.DSTypeID == 3 ?
                            dsTransferOutTran.DSAccountName :
                            dsTransactionByAcc.DSItemName,
                        ID = dsTransactionByAcc.ID,
                        DSTypeID = dsTransactionByAcc.DSTypeID,
                        DSAccountID = dsTransactionByAcc.DSAccountID,
                        DSAccountToID = dsTransactionByAcc.DSTypeID == 3 ? dsTransferOutTran.DSAccountID : 0,
                        DSTransferOutID = dsTransactionByAcc.DSTransferOutID,
                        Description = dsTransactionByAcc.Description,
                        CreatedDateTime = dsTransactionByAcc.CreatedDateTime,
                        Amount = dsTransactionByAcc.Amount,
                        Balance = balance
                    }); ;
                }
            }
            var finalResOrdered = finalRes.OrderByDescending(x => x.CreatedDateTime).ThenByDescending(x => x.RowID);
            if (dateFrom == null || dateTo == null)
            {
                return finalResOrdered;
            }
            return finalResOrdered.Where(x => x.CreatedDateTime >= dateFrom && x.CreatedDateTime <= dateTo);
        }

        public async Task<DSTransactionDto> Add(DSTransactionReq req)
        {
            var entity = _mapper.Map<DSTransaction>(req);

            _context.DSTransactions.Add(entity);
            entity.MemberID = MemberId;
            _context.SaveChanges();

            if (req.DSTypeID == 3)
            {
                DSTransaction entityToAccount = new DSTransaction
                {
                    DSTypeID = 4,
                    Amount = entity.Amount,
                    Description = entity.Description,
                    DSAccountID = req.DSAccountToID,
                    DSTransferOutID = entity.ID,
                    CreatedDateTime = req.CreatedDateTime,
                    MemberID = MemberId
                };
                _context.DSTransactions.Add(entityToAccount);
                _context.SaveChanges();
            }

            return new DSTransactionDto();
        }

        public async Task<bool> Edit(int id, DSTransactionReq req)
        {
            var origin = _context.DSTransactions.FirstOrDefault(x => x.ID == id);
            if (origin == null)
            {
                throw new NotFoundException("Transaction Record not found");
            }

            if (req.DSTypeID == 3)
            {
                if (req.DSAccountToID == 0)
                {
                    throw new BadRequestException("Must insert a transfer to account");
                }
                else if (req.DSAccountToID == req.DSAccountID)
                {
                    throw new BadRequestException("Transfer out account cannot be same");
                }
            }

            if (_transferTypes.Contains(origin.DSTypeID))
            {
                var originFromToAccount = _context.DSTransactions.
                    FirstOrDefault(x => origin.DSTypeID == 3 ? x.DSTransferOutID == id : x.ID == origin.DSTransferOutID);

                if (!_transferTypes.Contains(req.DSTypeID)) //not transfer type
                {
                    _context.DSTransactions.Remove(originFromToAccount);
                    _mapper.Map(req, origin);
                }
                else
                {
                    origin.DSAccountID = origin.DSTypeID == 3 ? req.DSAccountID : req.DSAccountToID;
                    origin.Amount = req.Amount;
                    origin.Description = req.Description;

                    originFromToAccount.DSAccountID = origin.DSTypeID == 3 ? req.DSAccountToID : req.DSAccountID;
                    originFromToAccount.Amount = req.Amount;
                    originFromToAccount.Description = req.Description;
                }
            }
            else
            {
                _mapper.Map(req, origin);

                if (_transferTypes.Contains(origin.DSTypeID)) //not transfer type
                {
                    var toAccount = new DSTransaction
                    {
                        DSTransferOutID = origin.ID,
                        DSTypeID = 4,
                        DSAccountID = req.DSAccountToID,
                        Description = origin.Description,
                        Amount = origin.Amount,
                        MemberID = MemberId
                    };
                    _context.DSTransactions.Add(toAccount);
                }
            }

            _context.SaveChanges();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var origin = _context.DSTransactions.FirstOrDefault(x => x.ID == id);
            if (origin == null)
            {
                throw new NotFoundException("Transaction Record not found");
            }

            _context.DSTransactions.Remove(origin);

            if (_transferTypes.Contains(origin.DSTypeID))
            {
                var originFromToAcction = _context.DSTransactions.
                    FirstOrDefault(x => origin.DSTypeID == 3 ? x.DSTransferOutID == id : x.ID == origin.DSTransferOutID);
                if (originFromToAcction == null)
                {
                    throw new NotFoundException("Transaction Record not found");
                }
                _context.DSTransactions.Remove(originFromToAcction);
            }

            _context.SaveChanges();

            return true;
        }
    }
}
