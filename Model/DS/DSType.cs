using System.ComponentModel.DataAnnotations.Schema;
using demoAPI.Model.School;

namespace demoAPI.Model.DS
{
    public class DSType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }
        public string Name { get; set; }        
    }
}
