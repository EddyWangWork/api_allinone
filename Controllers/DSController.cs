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

        public DSController(DSContext context, IMapper mapper, IDSBLL dsBLL)
        {
            _context = context;
            _mapper = mapper;
            _dsBLL = dsBLL;
        }

        [HttpGet("GetTransactionGroupStat")]
        public async Task<IActionResult> GetDebitStat(int dstypeid)
        {
            var responses = (
                 from a in _context.DSTransactions
                 join b in _context.DSItems on a.DSItemID equals b.ID into bb
                 from b2 in bb.DefaultIfEmpty()
                 join c in _context.DSItemSubs on a.DSItemSubID equals c.ID into cc
                 from c2 in cc.DefaultIfEmpty()
                 join d in _context.DSItems on c2.DSItemID equals d.ID into dd
                 from d2 in dd.DefaultIfEmpty()
                 where a.DSTypeID == dstypeid
                 select new DSDebitStat
                 {
                     DSItemName = b2.ID > 0 ? b2.Name : $"{d2.Name}|{c2.Name}",
                     Amount = a.Amount,
                 }).ToListAsync();

            var res = await responses;
            res = res.GroupBy(x => x.DSItemName).Select(y => new DSDebitStat { DSItemName = y.First().DSItemName, Amount = y.Sum(x => x.Amount) }).ToList();
            return Ok(res);
        }

        [HttpGet("GetDSTransaction")]
        public async Task<IActionResult> GetDSTransactionAsync()
        {
            return Ok(await _dsBLL.GetDSTransactionAsync());
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(DSTransactionReq req)
        {
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

        [HttpPut(Name = "GetDSItems")]
        public async Task<IActionResult> PutAsync(int id, DSTransactionReq req)
        {
            var origin = _context.DSTransactions.FirstOrDefault(x => x.ID == id);
            if (origin == null)
            {
                var message = string.Format("Record not found");
                return NotFound(message);
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
                        Amount = origin.Amount
                    };
                    _context.DSTransactions.Add(toAccount);
                }
            }

            _context.SaveChanges();

            return Ok(await GetDSTransactionAsync());
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var origin = _context.DSTransactions.FirstOrDefault(x => x.ID == id);
            if (origin == null)
            {
                var message = string.Format("Record not found");
                return NotFound(message);
            }

            _context.DSTransactions.Remove(origin);

            if (_transferTypes.Contains(origin.DSTypeID))
            {
                var originFromToAcction = _context.DSTransactions.
                    FirstOrDefault(x => origin.DSTypeID == 3 ? x.DSTransferOutID == id : x.ID == origin.DSTransferOutID);
                if (originFromToAcction == null)
                {
                    var message = string.Format("Record not found");
                    return NotFound(message);
                }
                _context.DSTransactions.Remove(originFromToAcction);
            }

            _context.SaveChanges();

            return Ok(await GetDSTransactionAsync());
        }
    }
}
