using AutoMapper;
using demoAPI.BLL.Common;
using demoAPI.BLL.DSItems;
using demoAPI.Common.Enum;
using demoAPI.Common.Helper;
using demoAPI.Data.DS;
using demoAPI.Model;
using demoAPI.Model.DS;
using demoAPI.Model.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

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
    }
}
