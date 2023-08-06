using AutoMapper;
using demoAPI.BLL.Common;
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
    public class CommonController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICommonBLL _commonBLL;


        public CommonController(IMapper mapper, ICommonBLL commonBLL)
        {
            _mapper = mapper;
            _commonBLL = commonBLL;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("getDSTransTypes")]
        public async Task<IActionResult> GetDSTransTypes()
        {
            var response = await _commonBLL.GetDSTransTypes();
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("getTodolistTypes")]
        public async Task<IActionResult> GetTodolistTypes()
        {
            var response = await _commonBLL.GetTodolistTypes();
            return Ok(response);
        }
    }
}
