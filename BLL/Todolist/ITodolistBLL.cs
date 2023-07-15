using demoAPI.Model;

namespace demoAPI.BLL.Todolist
{
    public interface ITodolistBLL
    {
        Task<List<TodolistDto>> GetTodolistsUndone();
        Task<Model.Todolist> Add(TodolistAddReq req);
        Task<Model.Todolist> Edit(int id, TodolistAddReq req);
        Task<Model.Todolist> Delete(int id);
    }
}
