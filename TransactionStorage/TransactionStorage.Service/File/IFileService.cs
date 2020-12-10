using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using TransactionModel = TransactionStorage.Service.Models.TransactionModel;


namespace TransactionStorage.Service.File
{
    public interface IFileService
    {
        bool IsContentTypeCorrect(string contentType);
        List<TransactionModel> GetTransactions(IFormFile file);
    }
}
