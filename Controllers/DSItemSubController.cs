using AutoMapper;
using demoAPI.BLL.DSItems;
using demoAPI.BLL.Member;
using demoAPI.Common.Helper;
using demoAPI.Data.DS;
using demoAPI.Model;
using demoAPI.Model.DS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace demoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DSItemSubController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IDSItemSubBLL _dsItemSubBLL;


        public DSItemSubController(IMapper mapper, IDSItemSubBLL dsItemSubBLL)
        {
            _mapper = mapper;
            _dsItemSubBLL = dsItemSubBLL;
        }

        [HttpPost]
        public async Task<IActionResult> Add(DSItemSubAddReq req)
        {
            var response = await _dsItemSubBLL.Add(req);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, DSItemSubAddReq req)
        {
            var response = await _dsItemSubBLL.Edit(id, req);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _dsItemSubBLL.Delete(id);
            return Ok(response);
        }
    }
}
