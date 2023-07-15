using AutoMapper;
using demoAPI.BLL.Member;
using demoAPI.Common.Helper;
using demoAPI.Data.DS;
using demoAPI.Middleware;
using demoAPI.Model;
using demoAPI.Model.DS;
using demoAPI.Model.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace demoAPI.BLL.Todolist
{
    public class TodolistBLL : BaseBLL, ITodolistBLL
    {
        private readonly DSContext _context;
        private readonly IMapper _mapper;

        public TodolistBLL(DSContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<TodolistDto>> GetTodolistsUndone()
        {
            var responses = (
                 from a in _context.Todolists
                 join b in _context.TodolistsDone on a.ID equals b.TodolistID into bb
                 from b2 in bb.DefaultIfEmpty()
                 where
                    a.MemberID == MemberId &&
                    (b2.UpdateDate.Month != DateTime.Now.Month || b2.UpdateDate == null)
                 select new TodolistDto
                 {
                     ID = a.ID,
                     Name = a.Name,
                     CategoryID = a.CategoryID,
                     Description = a.Description ?? string.Empty,
                     UpdateDate = a.UpdateDate,
                 }).ToListAsync();

            var todolist = await responses;

            return todolist;
        }

        public async Task<Model.Todolist> Add(TodolistAddReq req)
        {
            var entity = _mapper.Map<Model.Todolist>(req);
            entity.MemberID = MemberId;

            _context.Todolists.Add(entity);
            _context.SaveChanges();

            return entity;
        }


        public async Task<Model.Todolist> Edit(int id, TodolistAddReq req)
        {
            var entity = _context.Todolists.FirstOrDefault(x => x.ID == id);

            if (entity == null)
            {
                throw new NotFoundException($"Todolist record not found");
            }

            _mapper.Map(req, entity);
            _context.SaveChanges();

            return entity;
        }

        public async Task<Model.Todolist> Delete(int id)
        {
            var entity = _context.Todolists.FirstOrDefault(x => x.ID == id);
            var deletedRecord = entity;

            if (entity == null)
            {
                throw new NotFoundException($"Todolist record not found");
            }

            _context.Todolists.Remove(entity);
            _context.SaveChanges();

            return deletedRecord;
        }
    }
}
