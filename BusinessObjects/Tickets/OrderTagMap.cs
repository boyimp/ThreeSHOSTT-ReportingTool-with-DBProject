using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreeS.Domain.Models.Menus;
using ThreeS.Infrastructure.Data;

namespace ThreeS.Domain.Models.Tickets
{
    public class OrderTagMap : AbstractMap
    {
        public int OrderTagGroupId { get; set; }
        public string MenuItemGroupCode { get; set; }
        public int MenuItemId { get; set; }
    }
}
