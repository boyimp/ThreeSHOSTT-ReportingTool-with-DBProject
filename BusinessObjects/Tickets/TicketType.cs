using System.Collections.Generic;
using ThreeS.Domain.Models.Accounts;
using ThreeS.Domain.Models.Settings;
using ThreeS.Infrastructure.Data;

namespace ThreeS.Domain.Models.Tickets
{
    public class TicketType : EntityClass, IOrderable
    {
        public TicketType()
        {
            _entityTypeAssignments = new List<EntityTypeAssignment>();
        }

        public int ScreenMenuId { get; set; }
        public virtual Numerator TicketNumerator { get; set; }
        public virtual Numerator OrderNumerator { get; set; }
        public virtual AccountTransactionType SaleTransactionType { get; set; }
        public bool TaxIncluded { get; set; }
        public int SortOrder { get; set; }
        public string UserString { get { return Name; } }

        private IList<EntityTypeAssignment> _entityTypeAssignments;
        public virtual IList<EntityTypeAssignment> EntityTypeAssignments
        {
            get { return _entityTypeAssignments; }
            set { _entityTypeAssignments = value; }
        }

        private static TicketType _default;
        public static TicketType Default { get { return _default ?? (_default = new TicketType { SaleTransactionType = AccountTransactionType.Default }); } }
    }
}
