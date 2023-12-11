using AutoMapper;
using demoAPI.BLL.DS;
using demoAPI.Data.DS;
using demoAPI.Model.DS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace demoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DSController : ControllerBase
    {
        private readonly DSContext _context;
        private readonly IMapper _mapper;
        private readonly IDSBLL _dsBLL;

        private readonly List<int> _transferTypes = new() { 3, 4 };
        private readonly List<int> _validTransTypes = new() { 1, 2, 3, 31, 41 };

        public DSController(DSContext context, IMapper mapper, IDSBLL dsBLL)
        {
            _context = context;
            _mapper = mapper;
            _dsBLL = dsBLL;
        }

        [HttpGet("getDSYearExpenses")]
        public async Task<IActionResult> GetDSYearExpenses(int year)
        {
            return Ok(await _dsBLL.GetDSYearExpensesAsync(year));
        }

        [HttpGet("getDSYearCreditDebitDiff")]
        public async Task<IActionResult> GetDSYearCreditDebitDiff(int year)
        {
            return Ok(await _dsBLL.GetDSYearCreditDebitDiffAsync(year));
        }

        [HttpGet("getDSMonthlyExpenses")]
        public async Task<IActionResult> GetDSMonthlyExpensesAsync(int year, int month)
        {
            return Ok(await _dsBLL.GetDSMonthlyExpensesAsync(year, month));
        }

        [HttpGet("getTransactionGroupStat")]
        public async Task<IActionResult> GetDebitStat(int dstypeid, int year, int month)
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
                    a.DSTypeID == dstypeid &&
                    (a.CreatedDateTime.Year == year && a.CreatedDateTime.Month == month)
                 select new DSDebitStat
                 {
                     DSItemName = b2.ID > 0 ? b2.Name : $"{d2.Name}|{c2.Name}",
                     Amount = a.Amount,
                 }).ToListAsync();

            var res = await responses;
            res = res.GroupBy(x => x.DSItemName).Select(y => new DSDebitStat { DSItemName = y.First().DSItemName, Amount = y.Sum(x => x.Amount) }).ToList();
            return Ok(res);
        }

        [HttpGet("getDSTransaction")]
        public async Task<IActionResult> GetDSTransactionAsync()
        {
            return Ok(await _dsBLL.GetDSTransactionAsync());
        }

        [HttpGet("getDSTransactionV2")]
        public async Task<IActionResult> GetDSTransactionAsyncV2()
        {
            return Ok(await _dsBLL.GetDSTransactionAsyncV2());
        }

        [HttpPost("getDSTransactionWithDate")]
        public async Task<IActionResult> GetDSTransactionWithDateAsync(DSTransactionWithDateReq req)
        {
            return Ok(await _dsBLL.GetDSTransactionAsyncV2(req.DateFrom, req.DateTo));
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(DSTransactionReq req)
        {
            if (!_validTransTypes.Contains(req.DSTypeID))
            {
                return BadRequest($"Invalid transaction type: {req.DSTypeID}");
            }

            if (req.DSTypeID == 3)
            {
                if (req.DSAccountToID == 0)
                {
                    return BadRequest("Must insert a transfer to account");
                }
                else if (req.DSAccountToID == req.DSAccountID)
                {
                    return BadRequest("Transfer out account cannot be same");
                }
            }

            return Ok(await _dsBLL.Add(req));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, DSTransactionReq req)
        {
            if (!_validTransTypes.Contains(req.DSTypeID))
            {
                return BadRequest($"Invalid transaction type: {req.DSTypeID}");
            }

            if (req.DSTypeID == 3)
            {
                if (req.DSAccountToID == 0)
                {
                    return BadRequest("Must insert a transfer to account");
                }
                else if (req.DSAccountToID == req.DSAccountID)
                {
                    return BadRequest("Transfer out account cannot be same");
                }
            }

            var result = await _dsBLL.Edit(id, req);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _dsBLL.Delete(id);
            return Ok(result);
        }
    }
}
