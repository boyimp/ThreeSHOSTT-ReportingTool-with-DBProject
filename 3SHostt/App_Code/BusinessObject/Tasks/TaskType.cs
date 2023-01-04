using System.Collections.Generic;
using ThreeS.Domain.Models.Entities;
using ThreeS.Infrastructure.Data;

namespace ThreeS.Domain.Models.Tasks
{
    public class TaskType : EntityClass
    {
        public TaskType()
        {
            _entityTypes = new List<EntityType>();
        }

        private IList<EntityType> _entityTypes;
        public virtual IList<EntityType> EntityTypes
        {
            get { return _entityTypes; }
            set { _entityTypes = value; }
        }
    }
}