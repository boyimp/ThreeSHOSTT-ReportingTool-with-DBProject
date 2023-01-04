using ThreeS.Domain.Models.Menus;
using ThreeS.Domain.Models.Tickets;
using ThreeS.Infrastructure.Data;

namespace ThreeS.Domain.Models.Settings
{
    public class PrinterMap : ValueClass
    {
        public int PrintJobId { get; set; }
        public string MenuItemGroupCode { get; set; }
        public int MenuItemId { get; set; }
        public int PrinterId { get; set; }
        public int PrinterTemplateId { get; set; }
    }
}
