using AutoMapper;
using demoAPI.BLL.Member;
using demoAPI.BLL.Todolist;
using demoAPI.Common.Helper;
using demoAPI.Data.DS;
using demoAPI.Middleware;
using demoAPI.Model.TodlistsDone;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace demoAPI.Controllers
{
    [ApiController]
    [ResponseCompressionAttribute]
    [Route("[controller]")]
    public class TodolistDoneController : ControllerBase
    {
        private readonly DSContext _context;
        private readonly IMapper _mapper;
        private readonly ITodolistDoneBLL _todolistDoneBLL;

        private readonly List<int> _transferTypes = new() { 3, 4 };

        public TodolistDoneController(DSContext context, IMapper mapper, ITodolistDoneBLL todolistDoneBLL)
        {
            _context = context;
            _mapper = mapper;
            _todolistDoneBLL = todolistDoneBLL;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await _todolistDoneBLL.Get();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Add(TodolistDoneAddReq req)
        {
            var response = await _todolistDoneBLL.Add(req);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, TodolistDoneEditReq req)
        {
            var response = await _todolistDoneBLL.Edit(id, req);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _todolistDoneBLL.Delete(id);
            return Ok(response);
        }
    }
}
