using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TransactionStorage.Data.Models;

namespace TransactionStorage.Data.TransactionProvider
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly TransactionContext context;

        public TransactionRepository(TransactionContext context)
        {
            this.context = context;
        }

        public void InsertTransaction(List<Transactions> transactions)
        {
            foreach (var transaction in transactions)
            {
                if (context.Transactions.Any(t => t.Id == transaction.Id))
                {
                    context.Transactions.Update(transaction);
                }
                else
                {
                    context.Transactions.Add(transaction);
                }
            }

            context.SaveChanges();
        }

        public List<Transactions> GetTransactions(TransactionCriteria criteria)
        {
            var result = new List<Transactions>();

            result = context.Transactions
                .Where(transaction => string.IsNullOrWhiteSpace(criteria.Currency) || criteria.Currency == transaction.Currency)
                .Where(transaction => string.IsNullOrWhiteSpace(criteria.Status) || criteria.Status == transaction.Status)
                .Where(transaction => criteria.StartDate == DateTime.MinValue && criteria.EndDate == DateTime.MinValue || transaction.Date >= criteria.StartDate && transaction.Date <= criteria.EndDate)
                .ToList();

            return result;
        }

        private bool IsMatchCriteria(TransactionCriteria criteria, Transactions transaction)
        {
            var currency = string.IsNullOrWhiteSpace(criteria.Currency) || criteria.Currency == transaction.Currency;
            var status = string.IsNullOrWhiteSpace(criteria.Status) || criteria.Status == transaction.Status;
            //var date = criteria.StartDate == null && criteria.EndDate == null || IsDatetimeInRange(criteria.StartDate, criteria.EndDate, transaction.Date);
            var date = true;

            return currency && status && date;
        }

        private bool IsDatetimeInRange(DateTime start, DateTime end, DateTime dateTime)
        {
            return dateTime >= start && dateTime <= end;
        }
    }
}
