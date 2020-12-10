using LINQtoCSV;
using System.Xml.Serialization;

namespace TransactionStorage.Service.Models
{
    public class TransactionModel
    {
        [CsvColumn(FieldIndex = 1, Name = "Transaction Identificator")]
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }

        [CsvColumn(FieldIndex = 2, Name = "Amount")]
        public double Amount { get; set; }

        [CsvColumn(FieldIndex = 3, Name = "Currency Code")]
        public string CurrencyCode { get; set; }

        [CsvColumn(FieldIndex = 4, Name = "Transaction Date")]
        [XmlElement(ElementName = "TransactionDate")]
        public string Date { get; set; }

        [CsvColumn(FieldIndex = 5, Name = "Status")]
        [XmlElement(ElementName = "Status")]
        public string Status { get; set; }

        [XmlElement(ElementName = "PaymentDetails")]
        public PaymentDetails PaymentDetails { get; set; }

        [XmlIgnore]
        public bool IsXml { get; set; }
    }

    public class PaymentDetails
    {
        [XmlElement(ElementName = "Amount")]
        public double Amount { get; set; }

        [XmlElement(ElementName = "CurrencyCode")]
        public string CurrencyCode { get; set; }
    }
}
