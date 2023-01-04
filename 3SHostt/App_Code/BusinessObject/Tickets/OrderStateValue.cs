using System;
using System.Runtime.Serialization;

namespace ThreeS.Domain.Models.Tickets
{
    [DataContract]
    public class OrderStateValue : IEquatable<OrderStateValue>
    {
        [DataMember(Name = "SN")]
        public string StateName { get; set; }
        [DataMember(Name = "S")]
        public string State { get; set; }
        [DataMember(Name = "SV")]
        public string StateValue { get; set; }
        [DataMember(Name = "OK", EmitDefaultValue = true)]
        public string OrderKey { get; set; }

        private static OrderStateValue _default;
        public static OrderStateValue Default
        {
            get { return _default ?? (_default = new OrderStateValue()); }
        }

        public bool Equals(OrderStateValue other)
        {
            if (other == null)
            {
                return false;
            }

            return other.State == State && other.StateName == StateName;
        }

        public override int GetHashCode()
        {
            return (StateName + "_" + StateValue).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var item = obj as OrderStateValue;
            return item != null && Equals(item);
        }
    }
    [DataContract]
    public class CalculationTypeValue : IEquatable<CalculationTypeValue>
    {
        [DataMember(Name = "CN")]
        public string CalculationTypeName { get; set; }
        [DataMember(Name = "ID")]
        public int ID { get; set; }
        [DataMember(Name = "QT")]
        public decimal Qty { get; set; }
        [DataMember(Name = "PR")]
        public decimal Price { get; set; }
        [DataMember(Name = "CK")]
        public string CalculationKey { get; set; }

        private static CalculationTypeValue _default;
        public static CalculationTypeValue Default
        {
            get { return _default ?? (_default = new CalculationTypeValue()); }
        }
        public bool Equals(CalculationTypeValue other)
        {
            if (other == null)
            {
                return false;
            }

            return other.ID == ID && other.CalculationTypeName == CalculationTypeName;
        }
        public override int GetHashCode()
        {
            return (ID + "_" + CalculationTypeName).GetHashCode();
        }
        public override bool Equals(object obj)
        {
            var item = obj as CalculationTypeValue;
            return item != null && Equals(item);
        }
    }
}