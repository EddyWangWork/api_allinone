using AutoMapper;
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
    public class MemberController : ControllerBase
    {
        private readonly DSContext _context;
        private readonly IMapper _mapper;
        private readonly IMemberBLL _memberBLL;

        private readonly List<int> _transferTypes = new() { 3, 4 };

        public MemberController(DSContext context, IMapper mapper, IMemberBLL memberBLL)
        {
            _context = context;
            _mapper = mapper;
            _memberBLL = memberBLL ?? throw new ArgumentNullException(nameof(memberBLL));
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(MemberLoginReq req)
        {
            var token = _memberBLL.Login(req.Name, req.Password);

            if (token.IsNullOrEmpty())
            {
                return Unauthorized("Username or password incorrect");
            }

            return Ok(new { token });
        }
    }
}
