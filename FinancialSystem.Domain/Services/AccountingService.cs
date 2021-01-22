using FinancialSystem.Domain.Enums;
using FinancialSystem.Domain.Exceptions;
using FinancialSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialSystem.Domain.Services
{
    public interface IAccountingService
    {
        Task<List<Transaction>> GetTransactionsHistory();
        Task<Transaction> GetTransactionById(Guid id);
        Task<Transaction> ProcessNewTransaction(TransactionType transactionType, decimal amount);
        Task<decimal> GetCurrentBalance();
    }

    internal class AccountingService : IAccountingService
    {
        private static readonly object locker = new object();
        private static readonly Account account = new Account();
        private static bool isTransactionInProgress = false;

        public Task<List<Transaction>> GetTransactionsHistory()
        {
            while (isTransactionInProgress) ;
            return Task.FromResult(account.Transactions);
        }

        public Task<Transaction> GetTransactionById(Guid id)
        {
            return Task.FromResult(account.Transactions.FirstOrDefault(e => e.Id == id));
        }

        public Task<Transaction> ProcessNewTransaction(TransactionType transactionType, decimal amount)
        {
            lock (locker)
            {
                isTransactionInProgress = true;

                if (transactionType == TransactionType.Debit && amount > account.Amount)
                {
                    isTransactionInProgress = false;
                    throw new DebitException("Unable to proceed. Actual amount is less than debit amount.");
                }

                var transaction = new Transaction
                {
                    AccountId = account.Id,
                    Amount = amount,
                    Type = transactionType
                };

                account.Amount = transactionType == TransactionType.Credit ?
                    account.Amount + amount :
                    account.Amount - amount;

                account.Transactions.Add(transaction);

                isTransactionInProgress = false;

                return Task.FromResult(transaction);
            }
        }

        public Task<decimal> GetCurrentBalance()
        {
            while (isTransactionInProgress) ;
            return Task.FromResult(account.Amount);
        }
    }
}
