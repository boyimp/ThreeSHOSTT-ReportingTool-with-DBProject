using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreeS.Domain.Models.Settings;
using ThreeS.Infrastructure;
using ThreeS.Infrastructure.Data;

namespace ThreeS.Domain.Models.Tickets
{
    public class TicketTag : EntityClass, IStringCompareable
    {
        public int TicketTagGroupId { get; set; }
        public string Display { get { return !string.IsNullOrEmpty(Name) ? Name : "X"; } }

        private static TicketTag _emptyTicketTag;
        public static TicketTag Empty
        {
            get { return _emptyTicketTag ?? (_emptyTicketTag = new TicketTag()); }
        }

        public string GetStringValue()
        {
            return Name;
        }
    }
}
