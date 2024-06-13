using AutoMapper;
using demoAPI.BLL.Common;
using demoAPI.BLL.Kanbans;
using demoAPI.BLL.Member;
using demoAPI.Common.Helper;
using demoAPI.Data.DS;
using demoAPI.Middleware;
using demoAPI.Model;
using demoAPI.Model.Kanbans;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace demoAPI.Controllers
{
    [ApiController]
    [ResponseCompressionAttribute]
    [Route("[controller]")]
    public class KanbanController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IKanbanBLL _kanbanBLL;


        public KanbanController(IMapper mapper, IKanbanBLL kanbanBLL)
        {
            _mapper = mapper;
            _kanbanBLL = kanbanBLL;
        }

        [HttpGet]        
        public async Task<IActionResult> GetKanban()
        {
            var response = await _kanbanBLL.GetKanban();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Add(KanbanAddReq req)
        {
            var response = await _kanbanBLL.Add(req);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, KanbanAddReq req)
        {
            var response = await _kanbanBLL.Edit(id, req);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _kanbanBLL.Delete(id);
            return Ok(response);
        }
    }
}
