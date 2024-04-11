using demoAPI.Model.DS;
using System.Collections.Concurrent;

namespace demoAPI.Model
{
    public class GlobalVars
    {
        public static ConcurrentDictionary<int, List<DSTransactionDtoV2>> DSTransactions { get; set; }
        public static ConcurrentDictionary<int, List<DSTransactionDto>> DSTransactionsAll { get; set; }
    }    
}
