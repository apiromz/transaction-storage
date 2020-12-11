using System;
using System.Collections.Generic;
using System.Text;

namespace TransactionStorage.Service.Models
{
    public enum TransactionStatusCsv
    {
        Approved,
        Failed,
        Finished
    }

    public enum TransactionStatusXml
    {
        Approved,
        Rejected,
        Done
    }
}
