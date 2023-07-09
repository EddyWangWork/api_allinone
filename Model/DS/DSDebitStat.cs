using System.ComponentModel.DataAnnotations.Schema;
using demoAPI.Model.School;

namespace demoAPI.Model.DS
{
    public class DSDebitStat
    {        
        public string DSItemName { get; set; }
        public decimal Amount { get; set; }
    }
}
