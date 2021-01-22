using System;
using System.Collections.Generic;

namespace FinancialSystem.Domain.Models
{
    public class Account
    {
        public Account()
        {
            Id = Guid.NewGuid();
            Transactions = new List<Transaction>();
        }

        public Guid Id { get; }
        public decimal Amount { get; set; }

        public List<Transaction> Transactions { get; }
    }
}
