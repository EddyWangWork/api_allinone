namespace demoAPI.BLL.Common
{
    public interface ICommonBLL
    {
        Task<object> GetDSTransTypes();
        Task<object> GetTodolistTypes();
    }
}
