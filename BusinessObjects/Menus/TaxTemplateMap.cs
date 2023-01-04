using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreeS.Infrastructure.Data;

namespace ThreeS.Domain.Models.Menus
{
    public class TaxTemplateMap : AbstractMap
    {
        public int TaxTemplateId { get; set; }
        public string MenuItemGroupCode { get; set; }
        public int MenuItemId { get; set; }
    }
}
