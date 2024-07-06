using AutoMapper;
using demoAPI.Common.Enum;
using demoAPI.Data.DS;
using demoAPI.Model.DS;
using demoAPI.Model.DS.Accounts;
using demoAPI.Model.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace demoAPI.BLL
{
    public class DSAccountBLL : BaseBLL, IDSAccountBLL
    {
        private readonly DSContext _context;
        private readonly IMapper _mapper;

        private readonly List<int> expensesList = new List<int> {
            (int)EnumDSTranType.Expense, (int)EnumDSTranType.TransferOut, (int)EnumDSTranType.DebitTransferOut };
        private readonly List<int> incomeList = new List<int> {
            (int)EnumDSTranType.Income, (int)EnumDSTranType.TransferIn, (int)EnumDSTranType.CreditTransferIn };

        public DSAccountBLL(DSContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<DSAccountDto>> GetDSAccounts()
        {
            var dsAccounts = new List<DSAccountDto>();

            var responses = await _context.DSAccounts.ToListAsync();

            dsAccounts.AddRange(responses.Select(x => new DSAccountDto
            {
                ID = x.ID,
                Name = x.Name,
                IsActive = x.IsActive,
                CreatedDateTime = DateTime.UtcNow,
            }));

            return dsAccounts;
        }

        public async Task<List<DSAccountDto>> GetDSAccounts(bool isActive)
        {
            var dsAccounts = new List<DSAccountDto>();

            var responses = await _context.DSAccounts.Where(x => x.IsActive == isActive).ToListAsync();

            dsAccounts.AddRange(responses.Select(x => new DSAccountDto
            {
                ID = x.ID,
                Name = x.Name,
                IsActive = x.IsActive,
                CreatedDateTime = DateTime.UtcNow,
            }));

            return dsAccounts;
        }

        public async Task<List<DSAccountDto>> GetDSAccountsWithBalance2()
        {
            var dsAccounts = new List<DSAccountDto>();

            var responses = (
                from a in _context.DSTransactions
                join b in _context.DSAccounts on a.DSAccountID equals b.ID
                where a.MemberID == MemberId
                select new
                {
                    b.ID,
                    DSAccountName = b.Name,
                    b.IsActive,
                    a.DSAccountID,
                    a.DSTypeID,
                    a.Amount,
                    a.CreatedDateTime
                }).ToListAsync();

            var transationsTemp = await responses;

            var dsAccountIDs = transationsTemp.DistinctBy(x => x.DSAccountID).Select(x => x.DSAccountID);

            foreach (var dsAccountID in dsAccountIDs)
            {
                var incomes = transationsTemp.Where(x => x.DSAccountID == dsAccountID && incomeList.Contains(x.DSTypeID)).Sum(x => x.Amount);
                var expenses = transationsTemp.Where(x => x.DSAccountID == dsAccountID && expensesList.Contains(x.DSTypeID)).Sum(x => x.Amount);

                var transation = transationsTemp.OrderByDescending(x => x.CreatedDateTime).FirstOrDefault(x => x.DSAccountID == dsAccountID);

                dsAccounts.Add(new DSAccountDto
                {
                    ID = dsAccountID,
                    Name = transation.DSAccountName,
                    IsActive = transation.IsActive,
                    Balance = incomes - expenses,
                    CreatedDateTime = transation.CreatedDateTime,
                });
            }

            return dsAccounts;
        }

        public async Task<List<DSAccountDto>> GetDSAccountsWithBalance()
        {
            var dsAccounts = new List<DSAccountDto>();
            var dsAccountsOrdered = new List<DSAccountDto>();

            var dsAccountList = await _context.DSAccounts.ToListAsync();
            var dsTransList = await _context.DSTransactions.ToListAsync();

            foreach (var dsAccount in dsAccountList)
            {
                var dsAccountDto = new DSAccountDto
                {
                    ID = dsAccount.ID,
                    Name = dsAccount.Name,
                    IsActive = dsAccount.IsActive
                };

                var dsAccountSelected = dsTransList.Where(x => x.DSAccountID == dsAccount.ID);

                if (!dsAccountSelected.Any())
                {
                    dsAccounts.Add(dsAccountDto);
                    continue;
                }

                var incomes = dsAccountSelected.Where(x => incomeList.Contains(x.DSTypeID)).Sum(x => x.Amount);
                var expenses = dsAccountSelected.Where(x => expensesList.Contains(x.DSTypeID)).Sum(x => x.Amount);
                var LatestCreatedDateTime = dsAccountSelected.OrderByDescending(x => x.CreatedDateTime).FirstOrDefault().CreatedDateTime;

                dsAccountDto.Balance = incomes - expenses;
                dsAccountDto.CreatedDateTime = LatestCreatedDateTime;

                dsAccounts.Add(dsAccountDto);
            }

            dsAccountsOrdered.AddRange(dsAccounts.Where(x=>x.IsActive == true).OrderByDescending(x=>x.Balance));
            dsAccountsOrdered.AddRange(dsAccounts.Where(x=>x.IsActive != true).OrderByDescending(x=>x.Balance));

            return dsAccountsOrdered;
        }

        public async Task<DSAccount> Add(DSAccountAddReq req)
        {
            if (_context.DSAccounts.Any(x => x.Name == req.Name))
            {
                throw new BadRequestException($"DS Account record duplicated");
            }

            var entity = _mapper.Map<DSAccount>(req);
            entity.MemberID = MemberId;

            _context.DSAccounts.Add(entity);

            _context.SaveChanges();

            return entity;
        }

        public async Task<DSAccount> Edit(int id, DSAccountAddReq req)
        {
            var entity = _context.DSAccounts.FirstOrDefault(x => x.ID == id) ?? throw new NotFoundException($"DS Account record not found");

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

        public async Task<DSAccount> Delete(int id)
        {
            var entity = _context.DSAccounts.FirstOrDefault(x => x.ID == id) ?? throw new NotFoundException($"DS Account record not found");

            var deletedRecord = entity;

            _context.DSAccounts.Remove(entity);
            _context.SaveChanges();

            return deletedRecord;
        }
    }
}
