using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreeS.Infrastructure.Data;

namespace ThreeS.Domain.Models.Settings
{
    public class ForeignCurrency : EntityClass
    {
        public string CurrencySymbol { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal Rounding { get; set; }
    }
}
