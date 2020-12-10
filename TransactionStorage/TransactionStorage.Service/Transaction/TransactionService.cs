using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using TransactionStorage.Service.File;
using TransactionStorage.Service.Models;
using TransactionStorage.Service.Validation;

namespace TransactionStorage.Service.Transaction
{
    public class TransactionService : ITransactionService
    {
        private readonly AppSettings settings;
        private readonly IFileService fileService;
        private readonly IValidatorService validatorService;

        public TransactionService(IOptions<AppSettings> settings, IFileService fileService, IValidatorService validatorService)
        {
            this.settings = settings.Value;
            this.fileService = fileService;
            this.validatorService = validatorService;
        }

        public ValidationResult SaveTransaction(IFormFile file)
        {
            var transactions = fileService.GetTransactions(file);

            var validationResults = new List<ValidationResult>();
            for (var index = 0; index < transactions.Count; index++)
            {
                var validationResult = validatorService.Validate(transactions[index], index + 1);
                validationResults.Add(validationResult);
            }

            var result = new ValidationResult
            {
                Message = validationResults.SelectMany(v => v.Message).ToList()
            };
            result.IsValid = !result.Message.Any();

            if (!result.IsValid)
            {
                return result;
            }

            return result;
        }
    }
}
