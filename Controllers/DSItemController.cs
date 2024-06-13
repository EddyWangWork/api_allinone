using AutoMapper;
using demoAPI.BLL.DSItems;
using demoAPI.BLL.Member;
using demoAPI.Common.Helper;
using demoAPI.Data.DS;
using demoAPI.Middleware;
using demoAPI.Model;
using demoAPI.Model.DS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace demoAPI.Controllers
{
    [ApiController]
    [ResponseCompressionAttribute]
    [Route("[controller]")]
    public class DSItemController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IDSItemBLL _dsItemBLL;


        public DSItemController(IMapper mapper, IDSItemBLL dsItemBLL)
        {
            _mapper = mapper;
            _dsItemBLL = dsItemBLL;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await _dsItemBLL.GetDSItems();
            return Ok(response);
        }

        [HttpGet]
        [Route("getDSItemWithSub")]
        public async Task<IActionResult> GetDSItemWithSubV2()
        {
            var response = await _dsItemBLL.GetDSItemWithSubV2();
            return Ok(response);
        }

        [HttpGet]
        [Route("getDSItemWithSubV3")]
        public async Task<IActionResult> GetDSItemWithSubV3()
        {
            var response = await _dsItemBLL.GetDSItemWithSubV3();
            return Ok(response);
        }

        [HttpPost]
        [Route("addWithSubItem")]
        public async Task<IActionResult> AddWithSubItem(DSItemAddWithSubItemReq req)
        {
            var response = await _dsItemBLL.AddWithSubItem(req);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Add(DSItemAddReq req)
        {
            var response = await _dsItemBLL.Add(req);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, DSItemAddReq req)
        {
            var response = await _dsItemBLL.Edit(id, req);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _dsItemBLL.Delete(id);
            return Ok(response);
        }
    }
}
