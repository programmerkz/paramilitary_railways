using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RWS.Domain.Entities.Common;

namespace RWS.Domain.Entities
{
    public class TrainStation : AuditableBaseEntity<Guid>
    {
        public string Name { get; set; }
    }
}
