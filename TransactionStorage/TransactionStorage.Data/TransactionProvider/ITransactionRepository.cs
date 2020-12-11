using System;
using System.Collections.Generic;
using System.Text;
using TransactionStorage.Data.Models;

namespace TransactionStorage.Data.TransactionProvider
{
    public interface ITransactionRepository
    {
        void InsertTransaction(List<Transactions> transactions);
        List<Transactions> GetTransactions(TransactionCriteria criteria);
    }
}
