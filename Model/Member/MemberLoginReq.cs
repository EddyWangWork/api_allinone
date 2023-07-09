using System.ComponentModel.DataAnnotations.Schema;

namespace demoAPI.Model
{
    public class MemberLoginReq
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
