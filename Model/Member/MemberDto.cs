namespace demoAPI.Model
{
    public class MemberDto
    {
        public int ID { get; set; }
        public string Name { get; set; }        
        public string? Token { get; set; }
        public DateTime LastLoginDate { get; set; }
    }
}
