using AutoMapper;
using demoAPI.BLL;
using demoAPI.BLL.Member;
using demoAPI.Common.Helper;
using demoAPI.Data.DS;
using demoAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace demoAPI.Controllers
{
    [ApiController]
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

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await _dsAccountBLL.GetDSAccounts();
            return Ok(response);
        }
    }
}
