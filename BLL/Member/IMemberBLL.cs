namespace demoAPI.BLL.Member
{
    public interface IMemberBLL
    {
        string Login(string name, string password);
        Model.Member GetMemberByToken(string token);
        void UpdateMemberSession();
    }
}
