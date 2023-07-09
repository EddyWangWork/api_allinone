using System.ComponentModel.DataAnnotations.Schema;

namespace demoAPI.Model.DS
{
    public class DSItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int MemberID { get; set; }

        public ICollection<DSItemSub> DSItemSubs { get; set; }
        public Member Member { get; set; }
    }
}
