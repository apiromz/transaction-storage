using System;
using System.Collections.Generic;
using System.Text;

namespace TransactionStorage.Service.Models
{
    public class RetrieveTransaction
    {
        public string Id { get; set; }
        public string Payment { get; set; }
        public string Status { get; set; }
    }
}
