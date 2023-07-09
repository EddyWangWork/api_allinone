using System.ComponentModel.DataAnnotations.Schema;

namespace demoAPI.Model.DS
{
    public class TransProfile
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
