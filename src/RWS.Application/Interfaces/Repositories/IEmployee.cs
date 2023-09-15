using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RWS.Application.Interfaces.Repositories.Common;
using RWS.Domain.Entities;

namespace RWS.Application.Interfaces.Repositories
{
    public interface IEmployeeRepository : IGenericRepository<Employee, Guid>
    {
        public Task<bool> Exists(Guid id);
        public Task<int> Count();
    }
}
