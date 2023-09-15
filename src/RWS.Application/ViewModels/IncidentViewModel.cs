using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RWS.Domain.Entities.Common;

namespace RWS.Application.ViewModels
{
    public class IncidentViewModel
    {
        public Guid Id { get; set; }
        public DateTime SendingDateTime { get; set; }
        public bool IsSend { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }

        public Guid TrainStationId { get; set; }
        public Guid IncidentTypeId { get; set; }
        public Guid EmployeeId { get; set; }

        public virtual TrainStationViewModel TrainStation { get; set; }
        public virtual IncidentTypeViewModel IncidentType { get; set; }
        public virtual EmployeeViewModel Employee { get; set; }
    }
}
