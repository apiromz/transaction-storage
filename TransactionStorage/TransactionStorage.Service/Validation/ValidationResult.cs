using System;
using System.Collections.Generic;
using System.Text;

namespace TransactionStorage.Service.Validation
{
    public class ValidationResult
    {
        public List<string> Message { get; set; } = new List<string>();
        public bool IsValid { get; set; }
    }
}
