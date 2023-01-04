using System.Collections.Generic;
using ThreeS.Domain.Models.Accounts;
using ThreeS.Infrastructure.Data;

namespace ThreeS.Domain.Models.Tickets
{
    public class CalculationType : EntityClass, IOrderable
    {
        public int SortOrder { get; set; }

        public string UserString
        {
            get { return Name; }
        }

        public int CalculationMethod { get; set; }
        public decimal Amount { get; set; }
        public decimal MaxAmount { get; set; }
        public bool IncludeTax { get; set; }
        public bool DecreaseAmount { get; set; }
        public bool UsePlainSum { get; set; }
        public int AccountTransactionType_Id { get; set; }
        public bool IsDynamic { get; set; }
        public bool IsServiceCharge { get; set; }
        public bool IsTax { get; set; }
        public bool IsDiscount { get; set; }
        public bool IsSD { get; set; }
        public bool IncludeOtherCalculations { get; set; }
        public virtual AccountTransactionType AccountTransactionType { get; set; }

        private IList<CalculationTypeMap> _calculationTypeMaps;
        public virtual IList<CalculationTypeMap> CalculationTypeMaps
        {
            get { return _calculationTypeMaps; }
            set { _calculationTypeMaps = value; }
        }

        public CalculationType()
        {
            _calculationTypeMaps = new List<CalculationTypeMap>();
        }
    }
}
