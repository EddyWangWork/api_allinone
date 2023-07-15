using demoAPI.Model.TodlistsDone;

namespace demoAPI.BLL.Todolist
{
    public interface ITodolistDoneBLL
    {
        Task<List<TodolistDoneDto>> Get();
        Task<TodolistDone> Add(TodolistDoneAddReq req);
        Task<TodolistDone> Edit(int id, TodolistDoneEditReq req);
        Task<TodolistDone> Delete(int id);
    }
}
