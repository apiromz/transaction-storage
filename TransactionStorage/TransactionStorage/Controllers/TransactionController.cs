using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransactionStorage.API.Models;
using TransactionStorage.API.Models.Requests;
using TransactionStorage.Service.File;
using TransactionStorage.Service.Models;
using TransactionStorage.Service.Transaction;

namespace TransactionStorage.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly IFileService validationService;
        private readonly ITransactionService transactionService;

        public TransactionController(
            ILogger<TransactionController> logger, 
            IFileService validationService,
            ITransactionService transactionService)
        {
            _logger = logger;
            this.validationService = validationService;
            this.transactionService = transactionService;
        }

        [HttpPost]
        [Route("Upload")]
        public IActionResult Upload(IFormFile file)
        {
            if (!validationService.IsContentTypeCorrect(file.ContentType))
            {
                return Ok("Unknown format");
            }

            if (!validationService.IsFileSizeAllowed(file.Length))
            {
                return Ok("file size exceeds limit allowed (1MB)");
            }

            var result = transactionService.SaveTransaction(file);

            if (!result.IsValid)
            {
                return BadRequest(result.Message);
            }

            return Ok("Your transactions has been upload successfully!");
        }

        [HttpPost]
        [Route("GetTransactions")]
        public IActionResult GetTransactions(GetTransactionsRequest request)
        {
            var result = transactionService.GetTransactions(new TransactionCriteria
            {
                Currency = request.Currency,
                Status = request.Status,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
            });

            return Ok(result);
        }
    }
}
