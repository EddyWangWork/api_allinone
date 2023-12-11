using System.ComponentModel.DataAnnotations.Schema;

namespace demoAPI.Model.DS
{
    public class DSYearExpenses
    {
        public DSYearExpenses()
        {
            DSYearDetails = new List<DSYearDetails>();
        }

        public List<string> DSItemNames { get; set; }
        public List<DSYearDetails> DSYearDetails { get; set; }
    }

    public class DSYearDetails
    {
        public DSYearDetails()
        {
            Amount = new List<decimal>();
        }

        public string YearMonth { get; set; }
        public List<decimal> Amount { get; set; }
    }

    public class DSYearCreditDebitDiff
    {
        public string YearMonth { get; set; }
        public decimal Credit { get; set; }
        public decimal Debit { get; set; }
        public decimal Diff { get; set; }
    }

    public class DSDebitStat
    {
        public string DSItemName { get; set; }
        public decimal Amount { get; set; }
    }

    public class DSMonthlyExpenses
    {
        public string DSItemName { get; set; }
        public decimal Amount { get; set; }
    }
}
