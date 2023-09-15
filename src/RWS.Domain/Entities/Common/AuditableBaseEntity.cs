using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWS.Domain.Entities.Common
{
    public class AuditableBaseEntity<T> : IEntity
    {
        public virtual T Id { get; set; }
        object IEntity.Id { get { return Id; } set { } }
        public bool IsDeleted { get; set; } = false;
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? Deleted { get; set; }
        public string DeletedBy { get; set; }
    }
}
