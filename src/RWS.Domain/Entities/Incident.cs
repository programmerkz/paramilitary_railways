using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RWS.Domain.Entities.Common;

namespace RWS.Domain.Entities
{
    public class Incident : AuditableBaseEntity<Guid>
    {
        public DateTime SendingDateTime { get; set; }
        public bool IsSend { get; set; } = false;
        public string Title { get; set; }
        public string Text { get; set; }

        public Guid TrainStationId { get; set; }
        public virtual TrainStation TrainStation { get; set; }
        public Guid IncidentTypeId { get; set; }
        public virtual IncidentType IncidentType { get; set; }
        public Guid EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
