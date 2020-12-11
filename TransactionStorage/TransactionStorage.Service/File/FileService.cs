using LINQtoCSV;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using TransactionStorage.Service.Models;

namespace TransactionStorage.Service.File
{
    public class FileService : IFileService
    {
        private readonly List<string> acceptTypes = new List<string> {
            "application/octet-stream",
            "text/xml"
        };
        private readonly ILogger<FileService> logger;
        private readonly IOptions<AppSettings> setting;
        private const string ENCODING = "iso-8859-1";

        public FileService(ILogger<FileService> logger, IOptions<AppSettings> setting)
        {
            this.logger = logger;
            this.setting = setting;
        }

        public bool IsContentTypeCorrect(string contentType)
        {
            var contentTypeLowerCase = contentType.ToLower();

            return acceptTypes.Any(type => type.ToLower().Equals(contentTypeLowerCase));
        }

        public bool IsFileSizeAllowed(long filesize)
        {
            return filesize <= setting.Value.LimitFileUploadSize;
        }

        public List<TransactionModel> GetTransactions(IFormFile file)
        {
            if (file.ContentType.ToLower().Equals("application/octet-stream"))
            {
                return GetTransactionsCsv(file);
            }
            else
            {
                return GetTransactionsXml(file);
            }
        }

        public List<TransactionModel> GetTransactionsCsv(IFormFile file)
        {
            var transactions = new List<TransactionModel>();
            var inputFileDescription = new CsvFileDescription
            {
                SeparatorChar = ',',
                FirstLineHasColumnNames = false,
                IgnoreUnknownColumns = true,
                EnforceCsvColumnAttribute = true,
            };
            var csvContext = new CsvContext();

            try
            {
                using (var stream = file.OpenReadStream())
                {
                    var streamReader = new StreamReader(stream, Encoding.GetEncoding(ENCODING));
                    transactions = csvContext.Read<TransactionModel>(streamReader, inputFileDescription).ToList();
                }
            }
            catch (Exception ex)
            {
                logger.LogError("There's an exception occurs when trying to read csv file", ex);
                throw;
            }


            return transactions;
        }

        public List<TransactionModel> GetTransactionsXml(IFormFile file)
        {
            var transactions = new List<TransactionModel>();
            try
            {
                using (var stream = file.OpenReadStream())
                {
                    var xRoot = new XmlRootAttribute { 
                        IsNullable = true,
                        ElementName = "Transactions"
                    };
                    
                    var reader = new XmlSerializer(typeof(TransactionModels), xRoot);
                    var streamReader = new StreamReader(stream);
                    var model =  (TransactionModels) reader.Deserialize(streamReader);
                    transactions = model.Transaction;
                    transactions.ForEach(transaction =>
                    {
                        transaction.Amount = transaction.PaymentDetails.Amount;
                        transaction.CurrencyCode = transaction.PaymentDetails.CurrencyCode;
                        transaction.IsXml = true;
                    });
                }
            }
            catch (Exception ex)
            {
                logger.LogError("There's an exception occurs when trying to read xml file", ex);
                throw;
            }


            return transactions;
        }
    }
}
