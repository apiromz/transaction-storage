using System;
using System.Collections.Generic;
using System.Text;

namespace TransactionStorage.Service.Models
{
    public class AppSettings
    {
        public List<string> AcceptFileType { get; set; }
        public long LimitFileUploadSize { get; set; }
    }
}
