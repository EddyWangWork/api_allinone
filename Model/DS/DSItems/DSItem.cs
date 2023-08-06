using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mail;

namespace demoAPI.Model.DS
{
    //[Index(nameof(Name), IsUnique = true)]
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
