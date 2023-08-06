﻿using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly IMemberBLL _memberBLL;


        public MemberController(IMapper mapper, IMemberBLL memberBLL)
        {
            _mapper = mapper;
            _memberBLL = memberBLL;
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
