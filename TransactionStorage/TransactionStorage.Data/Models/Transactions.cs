using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TransactionStorage.Data.Models
{
    public class Transactions
    {
        [Key]
        public string Id { get; set; }

        public double Amount { get; set; }

        public string Currency { get; set; }

        public DateTime Date { get; set; }

        public string Status { get; set; }
    }
}
