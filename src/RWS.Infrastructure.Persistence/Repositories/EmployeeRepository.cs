using RWS.Application.Interfaces.Repositories;
using RWS.Domain.Entities;
using RWS.Infrastructure.Persistence.Contexts;
using RWS.Infrastructure.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace RWS.Infrastructure.Persistence.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee, Guid>, IEmployeeRepository
    {
        private readonly DbSet<Employee> _employees;

        public EmployeeRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _employees = dbContext.Set<Employee>();
        }

        public Task<bool> Exists(Guid id)
            => _employees.Where(t => t.Id == id && t.IsDeleted == false).AnyAsync();

        public Task<int> Count()
            => _employees.Where(t => t.IsDeleted == false).CountAsync();
    }
}
