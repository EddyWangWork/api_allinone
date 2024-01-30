using AutoMapper;
using demoAPI.Common.Enum;
using demoAPI.Data.DS;
using demoAPI.Model.Exceptions;
using demoAPI.Model.Kanbans;
using Microsoft.EntityFrameworkCore;

namespace demoAPI.BLL.Kanbans
{
    public class KanbanBLL : BaseBLL, IKanbanBLL
    {
        private readonly DSContext _context;
        private readonly IMapper _mapper;

        public KanbanBLL(DSContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<KanbanDto>> GetKanban()
        {
            var results = new List<KanbanDto>();

            try
            {
                var responses = await _context.Kanbans.ToListAsync();

                foreach (int kanbanStatus in Enum.GetValues(typeof(EnumKanbanStatus)))
                {
                    results.Add(new KanbanDto
                    {
                        Status = kanbanStatus,
                        KanbanDetails = responses.Where(x => x.Status == kanbanStatus).
                                                    OrderBy(x => x.Priority == 0 ? int.MaxValue : x.Priority).ThenByDescending(x=>x.UpdatedTime).ToList()
                    });
                }
                return results;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Kanban> Add(KanbanAddReq req)
        {
            var entity = _mapper.Map<Kanban>(req);
            entity.UpdatedTime = DateTime.Now;
            entity.MemberID = MemberId;

            _context.Kanbans.Add(entity);
            _context.SaveChanges();

            return entity;
        }

        public async Task<Kanban> Edit(int id, KanbanAddReq req)
        {
            var entity = await _context.Kanbans.FirstOrDefaultAsync(x => x.ID == id && x.MemberID == MemberId) ?? throw new NotFoundException($"Kanban record not found");
            entity.UpdatedTime = DateTime.Now;
            _mapper.Map(req, entity);

            _context.SaveChanges();
            return entity;
        }

        public async Task<Kanban> Delete(int id)
        {
            var entity = await _context.Kanbans.FirstOrDefaultAsync(x => x.ID == id) ?? throw new NotFoundException($"Todolist record not found");
            var deletedRecord = entity;

            _context.Kanbans.Remove(entity);
            _context.SaveChanges();

            return deletedRecord;
        }
    }
}
