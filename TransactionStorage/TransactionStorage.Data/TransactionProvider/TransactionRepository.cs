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
    }
}
