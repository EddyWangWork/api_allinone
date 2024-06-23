namespace demoAPI.Model
{
    public class ShopType
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public int MemberID { get; set; }
        public Member Member { get; set; }
    }
}
