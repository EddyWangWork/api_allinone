using System.ComponentModel.DataAnnotations.Schema;
using demoAPI.Model.School;

namespace demoAPI.Model.DS
{
    public class DSTransactionReq
    {
        public int DSTypeID { get; set; }
        public int DSAccountID { get; set; }
        public int DSAccountToID { get; set; }
        public int DSTransferOutID { get; set; }
        public int DSItemID { get; set; }
        public int DSItemSubID { get; set; }
        public string? Description { get; set; }
        public decimal Amount { get; set; }
    }
}
