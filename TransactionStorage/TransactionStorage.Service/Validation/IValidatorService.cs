using System;
using System.Collections.Generic;
using System.Text;
using TransactionStorage.Service.Models;

namespace TransactionStorage.Service.Validation
{
    public interface IValidatorService
    {
        ValidationResult Validate(TransactionModel transaction, int recordNumber);
    }
}
