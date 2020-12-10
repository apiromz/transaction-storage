using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using TransactionStorage.Service.File;
using TransactionStorage.Service.Models;

namespace TransactionStorage.Service.Transaction
{
    public class TransactionService : ITransactionService
    {
        private readonly AppSettings settings;
        private readonly IFileService fileService;

        public TransactionService(IOptions<AppSettings> settings, IFileService fileService)
        {
            this.settings = settings.Value;
            this.fileService = fileService;
        }

        public bool SaveTransaction(IFormFile file)
        {
            var transactions = fileService.GetTransactions(file);
            return true;
        }
    }
}
