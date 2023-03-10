using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ThreeS.Infrastructure.Data;

namespace ThreeS.Domain.Models.Tickets
{
    public class TicketEntity : ValueClass
    {
        public int EntityTypeId { get; set; }
        public int EntityId { get; set; }
        public int AccountId { get; set; }
        public int AccountTypeId { get; set; }
        public string EntityName { get; set; }
        public string EntityCustomData { get; set; }
        public string EntityTypeName { get; set; }
        public string CustomDataFromEntity { get; set; }


        public string GetCustomData(string fieldName)
        {
            if (string.IsNullOrEmpty(EntityCustomData)) return "";
            var pattern = string.Format("\"Name\":\"{0}\",\"Value\":\"([^\"]+)\"", fieldName);
            
            return Regex.IsMatch(EntityCustomData, pattern)
                ? Regex.Unescape(Regex.Match(EntityCustomData, pattern).Groups[1].Value) : "";
        }
        public string GetCustomDataFromEntity(string fieldName)
        {
            if (string.IsNullOrEmpty(CustomDataFromEntity)) return "";
            var pattern = string.Format("\"Name\":\"{0}\",\"Value\":\"([^\"]+)\"", fieldName);

            return Regex.IsMatch(CustomDataFromEntity, pattern)
                ? Regex.Unescape(Regex.Match(CustomDataFromEntity, pattern).Groups[1].Value) : "";
        }

        public string GetCustomDataFormat(string fieldName, string format)
        {
            var result = GetCustomData(fieldName.Trim());
            return !string.IsNullOrEmpty(result) ? string.Format(format, result) : "";
        }

        public decimal GetCustomDataAsDecimal(string fieldName)
        {
            decimal result;
            if (decimal.TryParse(GetCustomData(fieldName), out result))
                return result;
            return 0;
        }
    }
}
