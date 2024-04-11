using AutoMapper;
using demoAPI.Common.Enum;
using demoAPI.Common.Helper;
using demoAPI.Data.DS;
using demoAPI.Model;
using demoAPI.Model.DS;
using demoAPI.Model.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace demoAPI.BLL.DS
{
    public class DSBLL : BaseBLL, IDSBLL
    {
        private readonly DSContext _context;
        private readonly IMapper _mapper;

        private readonly List<int> _transferTypes = new() { 3, 4 };
        private readonly List<int> _creditDebitTypes = new() { 1, 2 };
        private readonly List<int> _creditItems = new() { 19 };
        private readonly List<int> _debitItems = new() { 1 }; //commitment:1

        public DSBLL(DSContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region Dashboard

        public async Task<IEnumerable<DSMonthlyItemExpenses>> GetDSMonthlyItemExpensesAsync(int year, int month, int monthDuration)
        {
            var dateCurrent = new DateTime(year, month, 1).AddMonths(1);
            var datePrev = dateCurrent.AddMonths(-monthDuration);

            var transactionAll = await GetSetDSTransactionAllAsync();
            transactionAll = transactionAll.Where(x => x.DSTypeID == 2 && x.CreatedDateTime >= datePrev && x.CreatedDateTime < dateCurrent);

            var distinctDatetime = transactionAll.GroupBy(x => x.CreatedDateTimeYearMonth).Select(x => x.First().CreatedDateTimeYearMonth).OrderByDescending(x => x).ToList();
            var resGroupbyName = transactionAll.GroupBy(x => new { x.CreatedDateTimeYearMonth, x.DSItemNameMain }).Select(y => new
            {
                y.FirstOrDefault().CreatedDateTimeYearMonth,
                y.FirstOrDefault().DSItemNameMain,
                Amount = y.Sum(x => x.Amount)
            }).ToList();
            var resGroupbySubName = transactionAll.GroupBy(x => new { x.CreatedDateTimeYearMonth, x.DSItemName }).Select(y => new
            {
                y.FirstOrDefault().CreatedDateTimeYearMonth,
                y.FirstOrDefault().DSItemName,
                Amount = y.Sum(x => x.Amount)
            }).ToList();

            var finalRes = new List<DSMonthlyItemExpenses>();

            for (int i = 0; i <= distinctDatetime.Count() - 2; i++)
            {
                var monthlyDatetime = distinctDatetime[i];
                var monthlyItems = new List<DSMonthlyItem>();
                var monthlySubItems = new List<DSMonthlyItem>();

                var allitems = resGroupbyName.Where(x => x.CreatedDateTimeYearMonth == distinctDatetime[i]).Select(x => x.DSItemNameMain).
                        Union(resGroupbyName.Where(x => x.CreatedDateTimeYearMonth == distinctDatetime[i + 1]).Select(x => x.DSItemNameMain)).OrderBy(x => x).ToList();
                var allsubitems = resGroupbySubName.Where(x => x.CreatedDateTimeYearMonth == distinctDatetime[i]).Select(x => x.DSItemName).
                        Union(resGroupbySubName.Where(x => x.CreatedDateTimeYearMonth == distinctDatetime[i + 1]).Select(x => x.DSItemName)).OrderBy(x => x).ToList();

                allitems.ForEach(x =>
                {
                    var amountCurrent = resGroupbyName.FirstOrDefault(y => y.CreatedDateTimeYearMonth == distinctDatetime[i] && y.DSItemNameMain == x)?.Amount ?? 0;
                    var amountLast = resGroupbyName.FirstOrDefault(y => y.CreatedDateTimeYearMonth == distinctDatetime[i + 1] && y.DSItemNameMain == x)?.Amount ?? 0;
                    var amountDiff = amountCurrent - amountLast;
                    var diffPercentageNumber = (amountCurrent == 0 || amountLast == 0) ?
                        amountCurrent == 0 ?
                        -100 : 100 :
                        (((amountCurrent / amountLast) - 1) * 100);
                    var diffPercentage = (amountCurrent == 0 || amountLast == 0) ?
                        amountCurrent == 0 ?
                        "-100" : "100" :
                        (((amountCurrent / amountLast) - 1) * 100).ToString("0.00");

                    monthlyItems.Add(new DSMonthlyItem
                    {
                        ItemName = x,
                        Amount = amountCurrent,
                        AmountLast = amountLast,
                        Diff = amountDiff,
                        DiffPercentageNumber = diffPercentageNumber,
                        AmountComparePercentage = diffPercentage
                    });
                });

                allsubitems.ForEach(x =>
                {
                    var amountCurrent = resGroupbySubName.FirstOrDefault(y => y.CreatedDateTimeYearMonth == distinctDatetime[i] && y.DSItemName == x)?.Amount ?? 0;
                    var amountLast = resGroupbySubName.FirstOrDefault(y => y.CreatedDateTimeYearMonth == distinctDatetime[i + 1] && y.DSItemName == x)?.Amount ?? 0;
                    var amountDiff = amountCurrent - amountLast;
                    var diffPercentageNumber = (amountCurrent == 0 || amountLast == 0) ?
                        amountCurrent == 0 ?
                        -100 : 100 :
                        (((amountCurrent / amountLast) - 1) * 100);
                    var diffPercentage = (amountCurrent == 0 || amountLast == 0) ?
                        amountCurrent == 0 ?
                        "-100" : "100" :
                        (((amountCurrent / amountLast) - 1) * 100).ToString("0.00");

                    monthlySubItems.Add(new DSMonthlyItem
                    {
                        ItemName = x,
                        Amount = amountCurrent,
                        AmountLast = amountLast,
                        Diff = amountDiff,
                        DiffPercentageNumber = diffPercentageNumber,
                        AmountComparePercentage = diffPercentage
                    });
                });

                monthlyItems.ForEach(x =>
                {
                    var monthlyExpensesItems = new List<DSMonthlyExpensesItem>();

                    transactionAll.Where(y => y.DSItemNameMain == x.ItemName && y.CreatedDateTimeYearMonth == monthlyDatetime).ToList().ForEach(yy =>
                    {
                        monthlyExpensesItems.Add(new DSMonthlyExpensesItem
                        {
                            ItemName = yy.DSItemNameSub,
                            Desc = yy.Description,
                            Amount = yy.Amount
                        });
                    });

                    x.ItemsDetail = monthlyExpensesItems.OrderByDescending(x => x.Amount).ToList();
                });

                finalRes.Add(new DSMonthlyItemExpenses
                {
                    YearMonthDatetime = monthlyDatetime,
                    DSMonthlyItems = monthlyItems.OrderByDescending(x => x.Diff).ToList(),
                    DSMonthlySubItems = monthlySubItems.OrderByDescending(x => x.Diff).ToList()
                });
            }

            return finalRes;
        }        

        public async Task<IEnumerable<DSMonthlyPeriodCreditDebit>> GetDSMonthlyPeriodCreditDebitAsync(int year, int month, int monthDuration,
            bool isIncludeCredit, List<int> creditIds, bool isIncludeDebit, List<int> debitIds)
        {
            var dateCurrent = new DateTime(year, month, 1).AddMonths(1);
            var datePrev = dateCurrent.AddMonths(-monthDuration);

            var transactionAll = await GetSetDSTransactionAllAsync();
            transactionAll = transactionAll.Where(x => _creditDebitTypes.Contains(x.DSTypeID) && x.CreatedDateTime >= datePrev && x.CreatedDateTime < dateCurrent);

            var resGroupby = transactionAll.GroupBy(x => new { x.CreatedDateTime.Year, x.CreatedDateTime.Month }).ToList();
            List<DSMonthlyPeriodCreditDebit> monthlyPeriodCreditDebit = new List<DSMonthlyPeriodCreditDebit>();

            resGroupby.ForEach(x =>
            {
                var yearMonthDatetime = new DateTime(x.Key.Year, x.Key.Month, 1);
                var yearMonth = $"{x.Key.Year}-{x.Key.Month}";

                var credit = creditIds.Count > 0 ?
                x.Where(x => x.DSTypeID == 1 && (isIncludeCredit ? creditIds.Contains(x.DSItemID) : !creditIds.Contains(x.DSItemID))).Sum(x => x.Amount) :
                x.Where(x => x.DSTypeID == 1).Sum(x => x.Amount);

                var debit = debitIds.Count > 0 ?
                x.Where(x => x.DSTypeID == 2 && (isIncludeDebit ? debitIds.Contains(x.DSItemID) : !debitIds.Contains(x.DSItemID))).Sum(x => x.Amount) :
                x.Where(x => x.DSTypeID == 2).Sum(x => x.Amount);

                var remain = credit - debit;
                var usage = (debit > 0 && credit > 0) ? ((debit / credit) * 100).ToString("0") : 0.ToString("0");

                monthlyPeriodCreditDebit.Add(new DSMonthlyPeriodCreditDebit
                {
                    YearMonthDatetime = yearMonthDatetime,
                    YearMonth = yearMonth,
                    Credit = credit,
                    Debit = debit,
                    Remain = remain,
                    Usage = usage
                });
            });

            monthlyPeriodCreditDebit = monthlyPeriodCreditDebit.OrderByDescending(x => x.YearMonthDatetime).ToList();

            for (int i = 0; i <= monthlyPeriodCreditDebit.Count - 2; i++)
            {
                var creditCompare = (monthlyPeriodCreditDebit[i].Credit <= 0 || monthlyPeriodCreditDebit[i + 1].Credit <= 0) ? 100.ToString("0.00") : (((monthlyPeriodCreditDebit[i].Credit / monthlyPeriodCreditDebit[i + 1].Credit) - 1) * 100).ToString("0.00");

                monthlyPeriodCreditDebit[i].CreditCompare = creditCompare;
                monthlyPeriodCreditDebit[i].DebitCompare = (((monthlyPeriodCreditDebit[i].Debit / monthlyPeriodCreditDebit[i + 1].Debit) - 1) * 100).ToString("0.00");
                monthlyPeriodCreditDebit[i].UsageCompare = (Convert.ToInt32(monthlyPeriodCreditDebit[i].Usage) - Convert.ToInt32(monthlyPeriodCreditDebit[i + 1].Usage)).ToString("0");
            }

            return monthlyPeriodCreditDebit;
        }        

        public async Task<DSMonthlyExpenses> GetDSMonthlyCommitmentAndOtherAsync(int year, int month, List<int> debitIds)
        {
            debitIds = debitIds.Count > 0 ? debitIds : _debitItems;
            var transactionAll = await GetSetDSTransactionAllAsync();

            var commitments =
                transactionAll.
                    Where(x => x.DSTypeID == 2 && debitIds.Contains(x.DSItemID) && x.CreatedDateTime.Year == year && x.CreatedDateTime.Month == month).
                    Select(xx => new DSMonthlyExpensesItem
                    {
                        ItemName = xx.DSItemNameSub,
                        Desc = xx.Description,
                        Amount = xx.Amount
                    }).OrderByDescending(x => x.Amount).ToList();

            var others =
                transactionAll.
                    Where(x => x.DSTypeID == 2 && !debitIds.Contains(x.DSItemID) && x.CreatedDateTime.Year == year && x.CreatedDateTime.Month == month).
                    GroupBy(xx => xx.DSItemNameMain).
                    Select(xxx => new DSMonthlyExpensesItem
                    {
                        ItemName = xxx.Key,
                        Amount = xxx.Sum(x => x.Amount)
                    }).OrderByDescending(x => x.Amount).ToList();

            var commitment = commitments;

            return new DSMonthlyExpenses()
            {
                Items = commitments,
                ItemsOther = others
            };
        }

        public async Task<IEnumerable<DSMonthlyExpensesItem>> GetDSMonthlyExpensesAsync(int year, int month, string name, bool isExclude = false)
        {
            var responses = (
                 from a in _context.DSTransactions
                 join b in _context.DSItems on a.DSItemID equals b.ID into bb
                 from b2 in bb.DefaultIfEmpty()
                 join c in _context.DSItemSubs on a.DSItemSubID equals c.ID into cc
                 from c2 in cc.DefaultIfEmpty()
                 join d in _context.DSItems on c2.DSItemID equals d.ID into dd
                 from d2 in dd.DefaultIfEmpty()
                 where
                    a.DSTypeID == (int)EnumDSTranType.Expense &&
                    (isExclude ? d2.Name != name : d2.Name == name) &&
                    (a.CreatedDateTime.Year == year && a.CreatedDateTime.Month == month)
                 select new DSMonthlyExpensesItem
                 {
                     ItemName = isExclude ? b2 != null ? b2.Name : d2.Name : c2.Name,
                     Desc = a.Description ?? "",
                     Amount = a.Amount,
                 }).ToListAsync();

            var res = await responses;
            var resGroupby = res.GroupBy(x => x.ItemName).Select(y => new DSMonthlyExpensesItem
            {
                ItemName = y.Key,
                Amount = y.Sum(x => x.Amount)
            });
            var finalRes = isExclude ? resGroupby.OrderByDescending(x => x.Amount) : res.OrderByDescending(x => x.Amount);
            return finalRes;
        }

        #endregion

        public async Task<DSYearExpenses> GetDSYearExpensesAsync(int year)
        {
            var responses = (
                 from a in _context.DSTransactions
                 join b in _context.DSItems on a.DSItemID equals b.ID into bb
                 from b2 in bb.DefaultIfEmpty()
                 join c in _context.DSItemSubs on a.DSItemSubID equals c.ID into cc
                 from c2 in cc.DefaultIfEmpty()
                 join d in _context.DSItems on c2.DSItemID equals d.ID into dd
                 from d2 in dd.DefaultIfEmpty()
                 where
                    a.DSTypeID == (int)EnumDSTranType.Expense &&
                    (a.CreatedDateTime.Year == year)
                 select new
                 {
                     DSItemName = b2.ID > 0 ? b2.Name : $"{d2.Name}",
                     DSYearMonthOri = a.CreatedDateTime,
                     DSYearMonth = $"{a.CreatedDateTime.Month}",
                     //DSYearMonth = $"{a.CreatedDateTime.Year}-{a.CreatedDateTime.Month}",
                     Amount = a.Amount,
                 }).ToListAsync();

            var res = await responses;
            var resGroupby = res.GroupBy(x => new { x.DSYearMonth, x.DSItemName }).
                Select(y => new
                {
                    YearMonth = y.First().DSYearMonth,
                    ItemName = y.First().DSItemName,
                    Amount = y.Sum(x => x.Amount)
                }).ToList();

            var distYearMonths = res.OrderBy(x => x.DSYearMonthOri).DistinctBy(x => x.DSYearMonth).Select(x => x.DSYearMonth);
            var distItemNames = res.OrderBy(x => x.DSItemName).DistinctBy(x => x.DSItemName).Select(x => x.DSItemName);
            DSYearExpenses dsYearExpenses = new DSYearExpenses { DSItemNames = distItemNames.ToList() };

            foreach (var yearMonth in distYearMonths)
            {
                DSYearDetails dsYearDetails = new DSYearDetails() { YearMonth = yearMonth };

                foreach (var distItemName in distItemNames)
                {
                    var amount = resGroupby.FirstOrDefault(x => x.YearMonth == yearMonth && x.ItemName == distItemName)?.Amount;
                    dsYearDetails.Amount.Add(amount ?? 0);
                }

                dsYearExpenses.DSYearDetails.Add(dsYearDetails);
            }
            return dsYearExpenses;
        }

        public async Task<IEnumerable<DSYearCreditDebitDiff>> GetDSYearCreditDebitDiffAsync(int year)
        {
            List<int> CreditDebitList = new List<int> { (int)EnumDSTranType.Income, (int)EnumDSTranType.Expense };

            var responses = (
                 from a in _context.DSTransactions
                 join b in _context.DSItems on a.DSItemID equals b.ID into bb
                 from b2 in bb.DefaultIfEmpty()
                 join c in _context.DSItemSubs on a.DSItemSubID equals c.ID into cc
                 from c2 in cc.DefaultIfEmpty()
                 join d in _context.DSItems on c2.DSItemID equals d.ID into dd
                 from d2 in dd.DefaultIfEmpty()
                 where
                    (a.DSTypeID == (int)EnumDSTranType.Expense ||
                    (a.DSTypeID == (int)EnumDSTranType.Income && _creditItems.Contains(b2 != null ? b2.ID : c2.DSItemID))) &&
                    (a.CreatedDateTime.Year == year)
                 select new
                 {
                     DSItemName = b2.ID > 0 ? b2.Name : $"{d2.Name}",
                     a.DSTypeID,
                     DSYearMonthOri = a.CreatedDateTime,
                     DSYearMonth = $"{a.CreatedDateTime.Year}-{a.CreatedDateTime.Month}",
                     Amount = a.Amount,
                 }).ToListAsync();

            var res = await responses;
            var resGroupby = res.GroupBy(x => new { x.DSYearMonth, x.DSTypeID }).
                Select(y => new
                {
                    YearMonth = y.First().DSYearMonth,
                    Type = y.First().DSTypeID,
                    Amount = y.Sum(x => x.Amount)
                }).ToList();
            List<DSYearCreditDebitDiff> dsYearCreditDebitDiff = new List<DSYearCreditDebitDiff>();
            var ssssff = res.DistinctBy(x => x.DSYearMonth);
            foreach (var yearMonth in res.OrderBy(x => x.DSYearMonthOri).DistinctBy(x => x.DSYearMonth).Select(x => x.DSYearMonth))
            {
                var credit = resGroupby.FirstOrDefault(x => x.YearMonth == yearMonth && x.Type == (int)EnumDSTranType.Income)?.Amount;
                var debit = resGroupby.FirstOrDefault(x => x.YearMonth == yearMonth && x.Type == (int)EnumDSTranType.Expense)?.Amount;
                var diff = credit - debit;

                dsYearCreditDebitDiff.Add(new DSYearCreditDebitDiff
                {
                    YearMonth = yearMonth,
                    Credit = credit ?? 0,
                    Debit = debit ?? 0,
                    Diff = diff ?? 0
                });
            }
            return dsYearCreditDebitDiff;
        }

        public async Task<IEnumerable<DSDebitStat>> GetDSMonthlyExpensesAsync(int year, int month)
        {
            var responses = (
                 from a in _context.DSTransactions
                 join b in _context.DSItems on a.DSItemID equals b.ID into bb
                 from b2 in bb.DefaultIfEmpty()
                 join c in _context.DSItemSubs on a.DSItemSubID equals c.ID into cc
                 from c2 in cc.DefaultIfEmpty()
                 join d in _context.DSItems on c2.DSItemID equals d.ID into dd
                 from d2 in dd.DefaultIfEmpty()
                 where
                    a.DSTypeID == (int)EnumDSTranType.Expense &&
                    (a.CreatedDateTime.Year == year && a.CreatedDateTime.Month == month)
                 select new DSDebitStat
                 {
                     DSItemName = b2.ID > 0 ? b2.Name : $"{d2.Name}",
                     Amount = a.Amount,
                 }).ToListAsync();

            var res = await responses;
            res = res.GroupBy(x => x.DSItemName).Select(y => new DSDebitStat { DSItemName = y.First().DSItemName, Amount = y.Sum(x => x.Amount) }).ToList();
            return res.OrderByDescending(x => x.Amount);
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
                     CreatedDateTime = a.CreatedDateTime,
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

        public async Task<IEnumerable<DSTransactionDtoV2>> GetDSTransactionAsyncV3(DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            var transactionAll = await GetSetDSTransactionAllAsync();

            var finalRes = new List<DSTransactionDtoV2>();

            var dsaccountids = transactionAll.DistinctBy(x => x.DSAccountID).Select(x => x.DSAccountID);
            List<int> expensesList =
                new List<int> { (int)EnumDSTranType.Expense, (int)EnumDSTranType.TransferOut, (int)EnumDSTranType.DebitTransferOut };
            int rowID = 0;

            foreach (var dsaccountid in dsaccountids)
            {
                decimal balance = 0;
                var dsTransactionsByAcc = transactionAll.Where(x => x.DSAccountID == dsaccountid).OrderBy(x => x.CreatedDateTime);

                foreach (var dsTransactionByAcc in dsTransactionsByAcc)
                {
                    var dsTransferOutTran = new DSTransactionDto();
                    if (dsTransactionByAcc.DSTypeID == 3)
                    {
                        dsTransferOutTran = transactionAll.FirstOrDefault(x => x.DSTransferOutID == dsTransactionByAcc.ID);
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

        public async Task<IEnumerable<DSTransactionDtoV2>> GetDSTransactionAsyncV2(DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            if (GlobalVars.DSTransactions.ContainsKey(MemberId) && GlobalVars.DSTransactions[MemberId].Count > 0)
            {
                if (dateFrom == null || dateTo == null)
                    return GlobalVars.DSTransactions[MemberId];
                return GlobalVars.DSTransactions[MemberId].Where(x => x.CreatedDateTime >= dateFrom && x.CreatedDateTime <= dateTo);
            }

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
            GlobalVars.DSTransactions.AddOrUpdate(MemberId, finalResOrdered.ToList());
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

            await SetGlobalDSTransactions();
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
            await SetGlobalDSTransactions();
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
            await SetGlobalDSTransactions();

            return true;
        }

        private async Task SetGlobalDSTransactions()
        {
            GlobalVars.DSTransactions.AddOrUpdate(MemberId, new List<DSTransactionDtoV2>());
            await GetDSTransactionAsyncV2(null, null);
        }

        private async Task<IEnumerable<DSTransactionDto>> GetSetDSTransactionAllAsync()
        {
            if (GlobalVars.DSTransactionsAll.ContainsKey(MemberId) && GlobalVars.DSTransactionsAll[MemberId].Count > 0)
            {
                return GlobalVars.DSTransactionsAll[MemberId];
            }

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
                     CreatedDateTime = a.CreatedDateTime,
                     CreatedDateTimeYearMonth = new DateTime(a.CreatedDateTime.Year, a.CreatedDateTime.Month, 1),
                     DSItemName = c.ID == 4 ? h2.Name : d2.ID > 0 ? d2.Name : $"{f2.Name}|{e2.Name}",
                     DSItemNameMain = d2 != null ? d2.Name : f2.Name,
                     DSItemNameSub = d2 != null ? d2.Name : e2.Name,
                     ID = a.ID,
                     DSTypeID = a.DSTypeID,
                     DSAccountID = a.DSAccountID,
                     DSTransferOutID = a.DSTransferOutID,
                     DSItemID = _transferTypes.Contains(c.ID) ? 0 : a.DSItemID > 0 ? a.DSItemID : e2 != null ? e2.DSItemID : 999,
                     DSItemSubID = a.DSItemSubID,
                     Description = a.Description,
                     Amount = a.Amount,
                 }).ToListAsync();

            var dsTransactions = await responses;
            GlobalVars.DSTransactionsAll.AddOrUpdate(MemberId, dsTransactions);
            return dsTransactions;
        }
    }
}
