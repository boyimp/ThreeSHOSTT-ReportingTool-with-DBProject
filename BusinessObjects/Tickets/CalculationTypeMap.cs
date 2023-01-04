using System.Collections.Generic;
using ThreeS.Domain.Models.Accounts;
using ThreeS.Infrastructure.Data;

namespace ThreeS.Domain.Models.Tickets
{
    public class CalculationTypeMap : AbstractMap
    {
        public int CalculationTypeId { get; set; }
        public string MenuItemGroupCode { get; set; }
        public int MenuItemId { get; set; }
        public int ZoneId { get; set; }
    }
}
