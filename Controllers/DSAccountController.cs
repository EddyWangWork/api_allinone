using AutoMapper;
using demoAPI.BLL;
using demoAPI.Middleware;
using demoAPI.Model.DS.Accounts;
using Microsoft.AspNetCore.Mvc;

namespace demoAPI.Controllers
{
    [ApiController]
    [ResponseCompressionAttribute]
    [Route("[controller]")]
    public class DSAccountController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IDSAccountBLL _dsAccountBLL;


        public DSAccountController(IMapper mapper, IDSAccountBLL dsAccountBLL)
        {
            _mapper = mapper;
            _dsAccountBLL = dsAccountBLL;
        }

        [HttpGet("getDSAccounts")]
        public async Task<IActionResult> GetDSAccounts()
        {
            var response = await _dsAccountBLL.GetDSAccounts();
            return Ok(response);
        }

        [HttpGet("getDSAccountsByStatus")]
        public async Task<IActionResult> GetDSAccountsByStatus(bool isActive)
        {
            var response = await _dsAccountBLL.GetDSAccounts(isActive);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await _dsAccountBLL.GetDSAccountsWithBalance();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Add(DSAccountAddReq req)
        {
            var response = await _dsAccountBLL.Add(req);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, DSAccountAddReq req)
        {
            var response = await _dsAccountBLL.Edit(id, req);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _dsAccountBLL.Delete(id);
            return Ok(response);
        }
    }
}
