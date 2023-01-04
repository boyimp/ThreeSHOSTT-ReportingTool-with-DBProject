using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ThreeS.Domain.Models.Accounts;
using ThreeS.Infrastructure.Data;
using ThreeS.Infrastructure.Helpers;

namespace ThreeS.Domain.Models.Tickets
{
    public class Calculation : ValueClass
    {
        public string Name { get; set; }
        public int Order { get; set; }
        public int CalculationTypeId { get; set; }
        public int TicketId { get; set; }
        public int AccountTransactionTypeId { get; set; }
        public int CalculationType { get; set; }
        public bool IncludeTax { get; set; }
        public bool Dynamic { get; set; }
        public bool IsTax { get; set; }
        public bool IsSD { get; set; }
        public bool IsServiceCharge { get; set; }
        public bool IsAuto { get; set; }
        public bool IsDiscount { get; set; }
        public bool IncludeOtherCalculations { get; set; }
        public bool DecreaseAmount { get; set; }
        public bool UsePlainSum { get; set; }
        public decimal Amount { get; set; }
        public decimal CalculationAmount { get; set; }
        public decimal CalculationAmountWithoutOtherCalculations { get; set; }
        public decimal CalculationSumValue { get; set; }
        public decimal CalculationSumValueWithoutOtherCalculations { get; set; }
        public string CalculationTypeMap { get; set; }
        public string UserName { get; set; }

        private string _includedCalculations;
        public string IncludedCalculations
        {
            get { return _includedCalculations; }
            set
            {
                _includedCalculations = value;
            }
        }
        private List<CalculationTypeTag> _calculationTypes;
        internal List<CalculationTypeTag> CalculationTypes
        {
            get { return _calculationTypes ?? (_calculationTypes = JsonHelper.Deserialize<List<CalculationTypeTag>>(IncludedCalculations)); }
        }
        private IList<CalculationTypeMap> _calculationTypeMaps;
        internal IList<CalculationTypeMap> CalculationTypeMaps
        {
            get { return _calculationTypeMaps ?? (_calculationTypeMaps = JsonHelper.Deserialize<List<CalculationTypeMap>>(CalculationTypeMap)); }
        }
        public decimal Calculate(decimal sum, decimal currentSum, int decimals)
        {
            decimal TempCalculationAmount = 0;
            if (CalculationType == 0)
            {
                TempCalculationAmount = Amount > 0 ? (sum * Amount) / 100 : 0;
            }
            else if (CalculationType == 1)
            {
                TempCalculationAmount = Amount > 0 ? (currentSum * Amount) / 100 : 0;
            }
            else if (CalculationType == 3)
            {
                if (Amount == currentSum) Amount = 0;
                else if (DecreaseAmount && Amount > currentSum)
                    Amount = 0;
                else if (!DecreaseAmount && Amount < currentSum)
                    Amount = 0;
                else
                    TempCalculationAmount = Amount - currentSum;
            }
            else if (CalculationType == 4)
            {
                if (Amount > 0)
                    TempCalculationAmount = (decimal.Round(currentSum / Amount, MidpointRounding.AwayFromZero) * Amount) - currentSum;
                else // eğer yuvarlama eksi olarak verildiyse hep aşağı yuvarlar
                    TempCalculationAmount = (Math.Truncate(currentSum / Amount) * Amount) - currentSum;
                if (DecreaseAmount && TempCalculationAmount > 0) TempCalculationAmount = 0;
                if (!DecreaseAmount && TempCalculationAmount < 0) TempCalculationAmount = 0;
            }
            else TempCalculationAmount = Amount;

            TempCalculationAmount = Decimal.Round(TempCalculationAmount, decimals);
            if (DecreaseAmount && TempCalculationAmount > 0) TempCalculationAmount = 0 - TempCalculationAmount;
            return TempCalculationAmount;
        }
        public List<CalculationTypeTag> GetMappedCalculationTypes()
        {
            return CalculationTypes;
        }
        public void Update(decimal sum, decimal sumWithoutOtherCalculations, decimal currentSum, int decimals)
        {
            CalculationSumValue = sum;
            CalculationAmountWithoutOtherCalculations = (decimal)0;
            CalculationSumValueWithoutOtherCalculations = sumWithoutOtherCalculations;
            if (CalculationType == 0)
            {
                CalculationAmount = Amount > 0 ? (sum * Amount) / 100 : 0;
                CalculationAmountWithoutOtherCalculations = Amount > 0 ? (sumWithoutOtherCalculations * Amount) / 100 : (decimal)0;
            }
            else if (CalculationType == 1)
            {
                CalculationAmount = Amount > 0 ? (currentSum * Amount) / 100 : 0;
                CalculationAmountWithoutOtherCalculations = Amount > 0 ? (currentSum * Amount) / 100 : (decimal)0;
            }
            else if (CalculationType == 2)
            {
                CalculationAmount = sum > 0 ? Amount : 0;
                CalculationAmountWithoutOtherCalculations = sumWithoutOtherCalculations > 0 ? Amount : (decimal)0;
            }
            else if (CalculationType == 3)
            {
                if (Amount == currentSum) Amount = 0;
                else if (DecreaseAmount && Amount > currentSum)
                    Amount = 0;
                else if (!DecreaseAmount && Amount < currentSum)
                    Amount = 0;
                else
                    CalculationAmount = Amount - currentSum;
            }
            else if (CalculationType == 4)
            {
                if (Amount > 0)
                    CalculationAmount = (decimal.Round(currentSum / Amount, MidpointRounding.AwayFromZero) * Amount) - currentSum;
                else // eğer yuvarlama eksi olarak verildiyse hep aşağı yuvarlar
                    CalculationAmount = (Math.Truncate(currentSum / Amount) * Amount) - currentSum;
                if (DecreaseAmount && CalculationAmount > 0) CalculationAmount = 0;
                if (!DecreaseAmount && CalculationAmount < 0) CalculationAmount = 0;
            }
            else CalculationAmount = Amount;

            CalculationAmount = Decimal.Round(CalculationAmount, decimals);
            if (DecreaseAmount && CalculationAmount > 0) CalculationAmount = 0 - CalculationAmount;
        }
        public decimal GetCalculationAmount(decimal sum, decimal sumWithoutOtherCalculations, decimal currentSum, int decimals)
        {
            //CalculationSumValue = sum;
            decimal ReturnCalclationAmount = 0;
            if (CalculationType == 0)
            {
                ReturnCalclationAmount = Amount > 0 ? (sum * Amount) / 100 : 0;
            }
            else if (CalculationType == 1)
            {
                ReturnCalclationAmount = Amount > 0 ? (currentSum * Amount) / 100 : 0;
            }
            else if (CalculationType == 2)
            {
                ReturnCalclationAmount = sum > 0 ? Amount / CalculationSumValue * sum : 0;
            }
            else if (CalculationType == 3)
            {
                if (Amount == currentSum) Amount = 0;
                else if (DecreaseAmount && Amount > currentSum)
                    Amount = 0;
                else if (!DecreaseAmount && Amount < currentSum)
                    Amount = 0;
                else
                    ReturnCalclationAmount = Amount - currentSum;
            }
            else if (CalculationType == 4)
            {
                if (Amount > 0)
                    ReturnCalclationAmount = (decimal.Round(currentSum / Amount, MidpointRounding.AwayFromZero) * Amount) - currentSum;
                else // eğer yuvarlama eksi olarak verildiyse hep aşağı yuvarlar
                    ReturnCalclationAmount = (Math.Truncate(currentSum / Amount) * Amount) - currentSum;
                if (DecreaseAmount && ReturnCalclationAmount > 0) ReturnCalclationAmount = 0;
                if (!DecreaseAmount && ReturnCalclationAmount < 0) ReturnCalclationAmount = 0;
            }
            else ReturnCalclationAmount = Amount;

            ReturnCalclationAmount = Decimal.Round(ReturnCalclationAmount, decimals);
            if (DecreaseAmount && ReturnCalclationAmount > 0) ReturnCalclationAmount = 0 - ReturnCalclationAmount;
            return ReturnCalclationAmount;
        }
        public void UpdateCalculationTransaction(AccountTransactionDocument document, decimal amount, decimal exchangeRate)
        {
            document.UpdateSingletonTransactionAmount(AccountTransactionTypeId, Name, amount, exchangeRate);
            if (amount == 0 && Amount == 0 && document.AccountTransactions.Any(x => x.AccountTransactionTypeId == AccountTransactionTypeId))
            {
                document.AccountTransactions.Remove(
                    document.AccountTransactions.Single(x => x.AccountTransactionTypeId == AccountTransactionTypeId));
            }
        }
    }
    public class CalculationTypeMap : AbstractMap
    {
        public int CalculationTypeId { get; set; }
        public string MenuItemGroupCode { get; set; }
        public int MenuItemId { get; set; }
        public int ZoneId { get; set; }
    }

    [DataContract]
    public class CalculationTypeTag
    {
        public CalculationTypeTag()
        {
            CalculationTypeId = 0;
            CalculationName = "";
        }
        public CalculationTypeTag(int Id, string Name)
        {
            CalculationTypeId = Id;
            CalculationName = Name;
        }
        [DataMember(Name = "CI")]
        public int CalculationTypeId { get; set; }
        [DataMember(Name = "CN")]
        public string CalculationName { get; set; }
        public static CalculationTypeTag GetCalculationType(IList<CalculationType> AllCalculationTyps, int CalculationTypeId)
        {
            var calType = AllCalculationTyps.SingleOrDefault(x => x.Id == CalculationTypeId);
            return calType != null ? new Tickets.CalculationTypeTag(calType.Id, calType.Name) : new Tickets.CalculationTypeTag();
        }
        public static IEnumerable<CalculationType> GetCalculationTypes(IEnumerable<CalculationType> AllCalculationTyps, IList<CalculationTypeTag> CalculationTypeTags)
        {
            return AllCalculationTyps.Where(x => CalculationTypeTags.SingleOrDefault(y => y.CalculationTypeId == x.Id) != null);
            //var calType = AllCalculationTyps.SingleOrDefault(x => x.Id == CalculationTypeId);
            //return calType != null ? new Tickets.CalculationTypeTag(calType.Id, calType.Name) : new Tickets.CalculationTypeTag();
        }
    }
}
