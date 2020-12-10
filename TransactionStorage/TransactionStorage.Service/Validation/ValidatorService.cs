using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TransactionStorage.Service.Models;

namespace TransactionStorage.Service.Validation
{
    public class ValidatorService : IValidatorService
    {
        public ValidationResult Validate(TransactionModel transaction, int recordNumber)
        {
            var result = new ValidationResult();

            var idValidation = new Validation("transaction id", transaction.Id);
            var amountValidation = new Validation("amount", transaction.Amount.ToString());
            var currencyValidation = new Validation("currencyCode", transaction.CurrencyCode);
            var dateValidation = new Validation("date", transaction.Date);
            var statusValidation = new Validation("status", transaction.Status);

            if (!idValidation.Required().IsText().MaxLength(50).Validate())
            {
                result.Message.AddRange(idValidation.result.Message);
            }

            if (!amountValidation.Required().IsDecimalNumber().Validate())
            {
                result.Message.AddRange(amountValidation.result.Message);
            }

            if (!currencyValidation.Required().IsCurrencySymbol().Validate())
            {
                result.Message.AddRange(currencyValidation.result.Message);
            }

            if (!dateValidation.Required().IsDateFormat(transaction.IsXml).Validate())
            {
                result.Message.AddRange(dateValidation.result.Message);
            }

            if (!statusValidation.Required().IsCorrectStatus(transaction.IsXml).Validate())
            {
                result.Message.AddRange(statusValidation.result.Message);
            }

            result.IsValid = !result.Message.Any();
            result.Message = result.Message.Select(msg => GenerateErrorMsgWithRecordNumber(msg, recordNumber)).ToList();
            return result;
        }

        private string GenerateErrorMsgWithRecordNumber(string msg, int recordNumber)
        {
            return $"[record : {recordNumber}], {msg}";
        }
    }
}
