using AutoMapper;
using demoAPI.Common.Helper;
using demoAPI.Data.DS;
using demoAPI.Model;

namespace demoAPI.BLL.Member
{
    public class MemberBLL : BLLBase, IMemberBLL
    {
        private readonly DSContext _context;
        private readonly IMapper _mapper;
        private readonly IJwtAuthenticationHelper _jwtAuthenticationHelper;

        public MemberBLL(
            DSContext context,
            IMapper mapper,
            IJwtAuthenticationHelper jwtAuthenticationHelper)
        {
            _context = context;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _jwtAuthenticationHelper = jwtAuthenticationHelper ?? throw new ArgumentNullException(nameof(jwtAuthenticationHelper));
        }

        public string Login(string name, string password)
        {
            var member = _context.Members.FirstOrDefault(x => x.Name == name && x.Password == password);
            if (member == null)
            {
                return string.Empty;
            }
            else
            {
                var token = _jwtAuthenticationHelper.GetToken(name);
                UpdateMemberToken(member, token);
                return token;
            }
        }

        public Model.Member GetMemberByToken(string token)
        {
            return _context.Members.FirstOrDefault(x => x.Token == token);
        }

        public void UpdateMemberSession()
        {
            var member = GetMember(MemberId);
            member.LastLoginDate = DateTime.Now;
            _context.SaveChanges();
        }

        #region PRIVATE

        public Model.Member GetMember(int id)
        {
            return _context.Members.FirstOrDefault(x => x.ID == id);
        }

        private void UpdateMemberToken(Model.Member member, string token)
        {
            member.Token = $"Bearer {token}";
            member.LastLoginDate = DateTime.Now;

            _context.Members.Update(member);
            _context.SaveChanges();
        }

        #endregion
    }
}
