using System.ComponentModel.DataAnnotations.Schema;

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
        public DateTime CreatedDateTime { get; set; }
    }

    public class DSTransactionWithDateReq
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
