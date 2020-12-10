using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using TransactionStorage.Data.Models;
using TransactionStorage.Data.TransactionProvider;
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
        private readonly ITransactionRepository transactionRepository;

        public TransactionService(IOptions<AppSettings> settings, IFileService fileService, IValidatorService validatorService, ITransactionRepository transactionRepository)
        {
            this.settings = settings.Value;
            this.fileService = fileService;
            this.validatorService = validatorService;
            this.transactionRepository = transactionRepository;
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

            var dataToSave = BuildDataEntity(transactions);

            transactionRepository.InsertTransaction(dataToSave);

            return result;
        }

        public List<RetrieveTransaction> GetTransactions(Service.Models.TransactionCriteria criteria)
        {
            var result = new List<RetrieveTransaction>();

            var transactions = transactionRepository.GetTransactions(new Data.Models.TransactionCriteria { 
                Currency = criteria.Currency, 
                Status = criteria.Status, 
                StartDate = criteria.StartDate, 
                EndDate = criteria.EndDate });

            result = transactions.Select(t => new RetrieveTransaction
            {
                Id = t.Id,
                Payment = $"{t.Amount} {t.Currency}",
                Status = t.Status,
            }).ToList();

            return result;
        }

        public List<Transactions> BuildDataEntity(List<TransactionModel> transactions)
        {
            return transactions.Select(transaction => new Transactions {
                Id = transaction.Id,
                Amount = double.Parse(transaction.Amount),
                Currency = transaction.CurrencyCode.ToUpper(),
                Date = transaction.IsXml ? DateTime.Parse(transaction.Date) : DateTime.ParseExact(transaction.Date, "dd/MM/yyyy hh:mm:ss", null),
                Status = GetStatus(transaction.Status, transaction.IsXml),
            }).ToList();
        }

        private string GetStatus(string status, bool isXml)
        {
            if (isXml)
            {
                var enumValue = (TransactionStatusXml) Enum.Parse(typeof(TransactionStatusXml), status);

                switch (enumValue)
                {
                    case TransactionStatusXml.Approved:
                        {
                            return "A";
                        }
                    case TransactionStatusXml.Rejected:
                        {
                            return "R";
                        }
                    case TransactionStatusXml.Done:
                        {
                            return "D";
                        }
                }
            }
            else
            {
                var enumValue = (TransactionStatusCsv) Enum.Parse(typeof(TransactionStatusCsv), status);
                switch (enumValue)
                {
                    case TransactionStatusCsv.Approved:
                        {
                            return "A";
                        }
                    case TransactionStatusCsv.Failed:
                        {
                            return "R";
                        }
                    case TransactionStatusCsv.Finished:
                        {
                            return "D";
                        }
                }
            }

            return "R";
        } 
    }
}
