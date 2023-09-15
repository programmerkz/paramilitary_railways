using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RWS.Application.Features.Incidents.Queries.GetAll;
using RWS.Application.Interfaces.Repositories.Common;
using RWS.Domain.Entities;

namespace RWS.Application.Interfaces.Repositories
{
    public interface IIncidentRepository : IGenericRepository<Incident, Guid>
    {
        public Task<Incident> GetByIdWithDictsAsync(Guid id);
        public Task<IReadOnlyList<Incident>> GetAllWithDictsPagedResponseAsync(int pageNumber, int pageSize);
        public Task<IReadOnlyList<Incident>> GetAllWithDictsPagedResponseAsync(int pageNumber, int pageSize, bool isSent);
        public Task<int> Count(GetAllIncidentsQuery request);
        public Task<IEnumerable<Incident>> GetAllAsync(GetAllIncidentsQuery request);
        public Task<int> Count(bool isSent);
    }
}
