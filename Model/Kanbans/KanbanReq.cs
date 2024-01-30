namespace demoAPI.Model.Kanbans
{
    public class KanbanAddReq
    {
        public int Type { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Status { get; set; }
        public int Priority { get; set; }
    }
}
