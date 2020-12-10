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
        private readonly IOptions<AppSettings> settings;
        private readonly IFileService validationService;
        private readonly ITransactionService transactionService;

        public TransactionController(
            ILogger<TransactionController> logger, 
            IOptions<AppSettings> settings,
            IFileService validationService,
            ITransactionService transactionService)
        {
            _logger = logger;
            this.settings = settings;
            this.validationService = validationService;
            this.transactionService = transactionService;
        }

        [HttpPost]
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
    }
}
