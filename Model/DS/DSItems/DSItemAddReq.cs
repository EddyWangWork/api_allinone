using System.ComponentModel.DataAnnotations.Schema;

namespace demoAPI.Model.DS
{
    public class DSItemAddReq
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }

    public class DSItemAddWithSubItemReq
    {
        public string Name { get; set; }
        public string? SubName { get; set; }
    }
}
