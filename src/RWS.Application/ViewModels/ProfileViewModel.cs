using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RWS.Domain.Entities.Common;

namespace RWS.Application.ViewModels
{
    public class ProfileViewModel
    {
        public string Id { get; set; }
        public string Login { get; set; }

        public Guid EmployeeId { get; set; }
        public virtual EmployeeViewModel Employee { get; set; }
    }
}
