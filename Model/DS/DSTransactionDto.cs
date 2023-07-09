using System.ComponentModel.DataAnnotations.Schema;
using demoAPI.Model.School;

namespace demoAPI.Model.DS
{
    public class DSTransactionDto : DSTransaction
    {
        public string DSTypeName { get; set; }
        public string DSAccountName { get; set; }
        public string DSItemName { get; set; }
    }
}
