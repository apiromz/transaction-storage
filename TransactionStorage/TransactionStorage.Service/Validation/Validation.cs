using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TransactionStorage.Service.Models;

namespace TransactionStorage.Service.Validation
{
    public class Validation
    {
        private readonly string name;
        private readonly string value;
        public readonly ValidationResult result;

        private const string REGEX_PATTERN_WORD_ONLY = "\\w*";

        public Validation(string name, string value)
        {
            this.name = name;
            this.value = value;
            result = new ValidationResult();
        }

        public Validation Required()
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                result.Message.Add($"{name} is required");
            }

            return this;
        }

        public Validation IsDecimalNumber()
        {
            if (!Decimal.TryParse(value, out _))
            {
                result.Message.Add($"{name} is incorrect format for decimal");
            }

            return this;
        }

        public Validation IsText()
        {
            var regex = new Regex(REGEX_PATTERN_WORD_ONLY);
            if (!regex.Match(value).Success)
            {
                result.Message.Add($"{name} include non-word charactor");
            }

            return this;
        }

        public Validation MaxLength(int max)
        {
            if (value.Length > max)
            {
                result.Message.Add($"{name} is over maxmimum {max} length");
            }

            return this;
        }

        public Validation IsCurrencySymbol()
        {
            var currencySymbols = CultureInfo.GetCultures(CultureTypes.SpecificCultures) //Only specific cultures contain region information
                        .Select(x => (new RegionInfo(x.LCID)).ISOCurrencySymbol.ToUpper())
                        .Distinct();
            if (!currencySymbols.Contains(value.ToUpper()))
            {
                result.Message.Add($"{name} is invalid");
            }

            return this;
        }

        public Validation IsDateFormat(bool isXml)
        {
            if (isXml)
            {
                return IsDateFormatXml();
            }

            return IsDateFormatCsv();
        }

        public Validation IsDateFormatCsv()
        {
            if (!DateTime.TryParseExact(value, "dd/MM/yyyy hh:mm:ss", null, DateTimeStyles.None, out _))
            {
                result.Message.Add($"{name} is invalid");
            }

            return this;
        }

        public Validation IsDateFormatXml()
        {
            if (!DateTime.TryParseExact(value, "yyyy-MM-ddThh:mm:ss", null, DateTimeStyles.None, out _))
            {
                result.Message.Add($"{name} is invalid");
            }

            return this;
        }

        public Validation IsCorrectStatus(bool isXml)
        {
            if (isXml)
            {
                return IsCorrectStatusXml();
            }

            return IsCorrectStatusCsv();
        }

        public Validation IsCorrectStatusXml()
        {
            if (!Enum.IsDefined(typeof(TransactionStatusXml), value))
            {
                result.Message.Add($"{name} is invalid");
            }
            return this;
        }

        public Validation IsCorrectStatusCsv()
        {
            if (!Enum.IsDefined(typeof(TransactionStatusCsv), value))
            {
                result.Message.Add($"{name} is invalid");
            }
            return this;
        }

        public bool Validate()
        {
            return !result.Message.Any();
        }
    }
}
