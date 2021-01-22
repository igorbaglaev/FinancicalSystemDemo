using FinancialSystem.Domain.Enums;
using System;

namespace FinancialSystem.Domain.Models
{
    public class Transaction
    {
        public Transaction()
        {
            Id = Guid.NewGuid();
            EffectiveDate = DateTimeOffset.UtcNow;
        }

        public Guid Id { get; }
        public Guid AccountId { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset EffectiveDate { get; }
    }
}
