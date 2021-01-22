using FinancialSystem.Domain.Enums;

namespace FinancialSystem.Web.Models.Request
{
    public class TransactionRequest
    {
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
    }
}
