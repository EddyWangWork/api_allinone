namespace demoAPI.Model.DS.Shops
{
    public class ShopAddReq
    {        
        public string Name { get; set; }
        public List<int> TypeList { get; set; }
        public string Location { get; set; }
        public string? Remark { get; set; }
        public string? Comment { get; set; }
        public int Star { get; set; }
        public bool IsVisited { get; set; }
    }
}
