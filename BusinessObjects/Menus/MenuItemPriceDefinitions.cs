using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using ThreeS.Domain.Models.Tickets;
using ThreeS.Infrastructure.Data;

namespace ThreeS.Domain.Models.Menus
{
    public class MenuItemPriceDefinition : EntityClass
    {
        [StringLength(10)]
        public string PriceTag { get; set; }
    }
}
