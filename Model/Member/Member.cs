using System.ComponentModel.DataAnnotations.Schema;

namespace demoAPI.Model
{
    public class Member
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string? Token { get; set; }
        public DateTime LastLoginDate { get; set; }
    }
}
