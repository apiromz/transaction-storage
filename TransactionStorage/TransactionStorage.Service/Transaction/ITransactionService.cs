using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace TransactionStorage.Service.Transaction
{
    public interface ITransactionService
    {
        bool SaveTransaction(IFormFile file);
    }
}
