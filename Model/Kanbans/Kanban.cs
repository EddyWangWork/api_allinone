namespace demoAPI.Model.Kanbans
{
    public class Kanban
    {
        public int ID { get; set; }
        public int Type { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Status { get; set; }
        public int Priority { get; set; }
        public DateTime UpdatedTime { get; set; }

        public int MemberID { get; set; }
        public Member Member { get; set; }
    }
}
