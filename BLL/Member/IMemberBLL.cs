using demoAPI.Model;

namespace demoAPI.BLL.Member
{
    public interface IMemberBLL
    {
        string Login(string name, string password);
        MemberDto LoginV2(string name, string password);
        Task<MemberDto> Edit(int id, MemberLoginReq req);
        Model.Member GetMemberByToken(string token);
        void UpdateMemberSession();
    }
}
