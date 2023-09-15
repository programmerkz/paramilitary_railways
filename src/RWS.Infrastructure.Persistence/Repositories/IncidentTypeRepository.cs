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
    public class IncidentTypeRepository : GenericRepository<IncidentType, Guid>, IIncidentTypeRepository
    {
        private readonly DbSet<IncidentType> _incidentTypes;

        public IncidentTypeRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _incidentTypes = dbContext.Set<IncidentType>();
        }

        public Task<bool> Exists(Guid id)
            => _incidentTypes.Where(t => t.Id == id && t.IsDeleted == false).AnyAsync();

        public Task<int> Count()
            => _incidentTypes.Where(t => t.IsDeleted == false).CountAsync();
    }
}
