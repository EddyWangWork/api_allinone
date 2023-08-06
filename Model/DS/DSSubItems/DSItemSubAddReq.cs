using System.ComponentModel.DataAnnotations.Schema;

namespace demoAPI.Model.DS
{
    public class DSItemSubAddReq
    {
        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public int DSItemID { get; set; }
    }
}
