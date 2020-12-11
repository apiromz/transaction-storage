using System;
using System.Collections.Generic;
using System.Text;

namespace TransactionStorage.Data.Models
{
    public class TransactionCriteria
    {
        public string Currency { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
    }
}
