using demoAPI.Model.Kanbans;

namespace demoAPI.BLL.Kanbans
{
    public interface IKanbanBLL
    {
        Task<List<KanbanDto>> GetKanban();
        Task<Kanban> Add(KanbanAddReq req);
        Task<Kanban> Edit(int id, KanbanAddReq req);
        Task<Kanban> Delete(int id);
    }
}
