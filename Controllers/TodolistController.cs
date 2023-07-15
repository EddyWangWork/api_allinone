using AutoMapper;
using demoAPI.BLL.Member;
using demoAPI.BLL.Todolist;
using demoAPI.Common.Helper;
using demoAPI.Data.DS;
using demoAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace demoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodolistController : ControllerBase
    {
        private readonly DSContext _context;
        private readonly IMapper _mapper;
        private readonly ITodolistBLL _todolistBLL;

        private readonly List<int> _transferTypes = new() { 3, 4 };

        public TodolistController(DSContext context, IMapper mapper, ITodolistBLL todolistBLL)
        {
            _context = context;
            _mapper = mapper;
            _todolistBLL = todolistBLL;
        }

        [HttpGet]
        public async Task<IActionResult> GetTodolistsUndone()
        {
            var response = await _todolistBLL.GetTodolistsUndone();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Add(TodolistAddReq req)
        {
            var response = await _todolistBLL.Add(req);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, TodolistAddReq req)
        {
            var response = await _todolistBLL.Edit(id, req);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _todolistBLL.Delete(id);
            return Ok(response);
        }
    }
}
