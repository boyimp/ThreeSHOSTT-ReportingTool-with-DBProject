using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreeS.Infrastructure.Data;

namespace ThreeS.Domain.Models.Tickets
{
    public class OrderTagTemplate : EntityClass
    {
        
        private IList<OrderTagTemplateValue> _orderTagTemplateValues;
        public virtual IList<OrderTagTemplateValue> OrderTagTemplateValues
        {
            get { return _orderTagTemplateValues; }
            set { _orderTagTemplateValues = value; }
        }

        public OrderTagTemplate()
        {
            _orderTagTemplateValues = new List<OrderTagTemplateValue>();
        }
    }
}
