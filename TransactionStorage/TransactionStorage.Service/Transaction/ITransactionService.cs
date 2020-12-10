using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using TransactionStorage.Service.Validation;

namespace TransactionStorage.Service.Transaction
{
    public interface ITransactionService
    {
        ValidationResult SaveTransaction(IFormFile file);
    }
}
