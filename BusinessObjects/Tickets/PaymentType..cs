using System.Collections.Generic;
using ThreeS.Domain.Models.Accounts;
using ThreeS.Infrastructure.Data;

namespace ThreeS.Domain.Models.Tickets
{
    public class PaymentType : EntityClass, IOrderable
    {
        public PaymentType()
        {
            _paymentTypeMaps = new List<PaymentTypeMap>();
        }

        public int SortOrder { get; set; }
        public string UserString { get { return Name; } }
        public string ButtonColor { get; set; }
        public virtual AccountTransactionType AccountTransactionType { get; set; }
        public virtual Account Account { get; set; }
        
        private IList<PaymentTypeMap> _paymentTypeMaps;
        public virtual IList<PaymentTypeMap> PaymentTypeMaps
        {
            get { return _paymentTypeMaps; }
            set { _paymentTypeMaps = value; }
        }

        public PaymentTypeMap AddPaymentTypeMap()
        {
            var map = new PaymentTypeMap();
            PaymentTypeMaps.Add(map);
            return map;
        }
    }
}
