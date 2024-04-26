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

    public class GetDSTransactionAsyncV2Req
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int DataLimit { get; set; }
    }

    public class DSTransactionWithDateReq
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }

    public class GetDSMonthlyPeriodCreditDebitReq
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int MonthDuration { get; set; }
        public bool IsIncludeCredit { get; set; }
        public List<int> CreditIds { get; set; }
        public bool IsIncludeDebit { get; set; }
        public List<int> DebitIds { get; set; }
    }

    public class GetDSMonthlyCommitmentAndOtherReq
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public List<int> DebitIds { get; set; }
    }
}