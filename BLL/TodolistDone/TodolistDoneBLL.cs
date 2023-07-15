using AutoMapper;
using demoAPI.BLL.Member;
using demoAPI.Common.Enum;
using demoAPI.Common.Helper;
using demoAPI.Data.DS;
using demoAPI.Middleware;
using demoAPI.Model;
using demoAPI.Model.DS;
using demoAPI.Model.Exceptions;
using demoAPI.Model.TodlistsDone;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace demoAPI.BLL.Todolist
{
    public class TodolistDoneBLL : BaseBLL, ITodolistDoneBLL
    {
        private readonly DSContext _context;
        private readonly IMapper _mapper;

        public TodolistDoneBLL(DSContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<TodolistDoneDto>> Get()
        {
            var responses = (
                 from a in _context.TodolistsDone
                 join b in _context.Todolists on a.TodolistID equals b.ID
                 select new TodolistDoneDto
                 {
                     ID = a.ID,
                     Remark = a.Remark,
                     UpdateDate = a.UpdateDate,
                     TodolistCategory = ((EnumTodolistType)b.CategoryID).ToString(),
                     TodolistDescription = b.Description,
                     TodolistName = b.Name,
                     TodolistID = b.ID
                 }).ToListAsync();

            var todolistdone = await responses;

            return todolistdone;
        }

        public async Task<TodolistDone> Add(TodolistDoneAddReq req)
        {
            var responses = (
                 from a in _context.Todolists
                 join b in _context.TodolistsDone on a.ID equals b.TodolistID into bb
                 from b2 in bb.DefaultIfEmpty()
                 where
                    a.ID == req.TodolistID
                 select new
                 {
                     TodolistID = a.ID,
                     TodolistDoneID = b2.ID != null ? b2.ID : 0,
                     a.CategoryID,
                     UpdateDate = b2.UpdateDate != null ? b2.UpdateDate : DateTime.MinValue,
                 }).FirstOrDefaultAsync();

            var todolist = await responses;

            if (todolist == null)
            {
                throw new BadRequestException($"Todolist record not found");
            }
            else if (todolist.TodolistDoneID == 0)
            {
                // no done record
            }
            else if (todolist?.CategoryID == (int)EnumTodolistType.Normal ||
                (todolist?.CategoryID == (int)EnumTodolistType.Monthly &&
                DateTime.Now.Year == todolist?.UpdateDate.Year &&
                DateTime.Now.Month == todolist?.UpdateDate.Month))
            {
                throw new BadRequestException("Todolist already done");
            }

            var entityDone = _mapper.Map<TodolistDone>(req);
            _context.TodolistsDone.Add(entityDone);
            _context.SaveChanges();

            return entityDone;
        }

        public async Task<TodolistDone> Edit(int id, TodolistDoneEditReq req)
        {
            var entity = _context.TodolistsDone.FirstOrDefault(x => x.ID == id);

            if (entity == null)
            {
                throw new NotFoundException($"Todolist done record not found");
            }

            _mapper.Map(req, entity);
            _context.SaveChanges();

            return entity;
        }

        public async Task<TodolistDone> Delete(int id)
        {
            var entity = _context.TodolistsDone.FirstOrDefault(x => x.ID == id);
            var deletedRecord = entity;

            if (entity == null)
            {
                throw new NotFoundException($"Todolist done record not found");
            }

            _context.TodolistsDone.Remove(entity);
            _context.SaveChanges();

            return deletedRecord;
        }
    }
}
