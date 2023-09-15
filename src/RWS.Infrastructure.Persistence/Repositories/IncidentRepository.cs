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
using RWS.Application.Features.Incidents.Queries.GetAll;

namespace RWS.Infrastructure.Persistence.Repositories
{
    public class IncidentRepository : GenericRepository<Incident, Guid>, IIncidentRepository
    {
        private readonly DbSet<Incident> _incidents;

        public IncidentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _incidents = dbContext.Set<Incident>();
        }

        public async Task<IReadOnlyList<Incident>> GetAllWithDictsPagedResponseAsync(int pageNumber, int pageSize)
            => await _incidents
            .Where(i => i.IsDeleted == false)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Include(i => i.IncidentType)
            .Include(i => i.TrainStation)
            .Include(i => i.Employee)
            .AsNoTracking()
            .ToListAsync();

        public async Task<Incident> GetByIdWithDictsAsync(Guid id)
            => await _incidents
            .Where(i => i.Id == id && i.IsDeleted == false)
            .Include(i => i.IncidentType)
            .Include(i => i.TrainStation)
            .Include(i => i.Employee)
            .FirstOrDefaultAsync();

        public async Task<IReadOnlyList<Incident>> GetAllWithDictsPagedResponseAsync(int pageNumber, int pageSize, bool isSent)
            => await _incidents
            .Where(i => i.IsSend == isSent && i.IsDeleted == false)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Include(i => i.IncidentType)
            .Include(i => i.TrainStation)
            .Include(i => i.Employee)
            .AsNoTracking()
            .ToListAsync();

        public async Task<int> Count(GetAllIncidentsQuery request) 
        {
            var count = await _incidents
                .Where(t => t.IsDeleted == false &&
                            t.IsSend == request.IsSend &&
                            (string.IsNullOrWhiteSpace(request.Name) || t.Title.ToLower().Contains(request.Name.ToLower())) &&
                            (request.IncidentTypeIds.Count == 0 || request.IncidentTypeIds.Contains(t.Id)) &&
                            (request.TrainStationIds.Count == 0 || request.TrainStationIds.Contains(t.Id)) &&
                            (request.IndcidentIds.Count == 0 || request.IndcidentIds.Contains(t.Id)) &&
                            ((request.DateFrom == default(DateTime) || (request.DateTo == default(DateTime)) || 
                             (t.SendingDateTime >= request.DateFrom && t.SendingDateTime <= request.DateTo))))
                .CountAsync();

            return count;
        } 

        public async Task<IEnumerable<Incident>> GetAllAsync(GetAllIncidentsQuery request)
        {
            var incidents = await _incidents
                .Include(t => t.IncidentType)
                .Include(t => t.TrainStation)
                .Include(t => t.Employee)
                .Where(t => t.IsDeleted == false &&
                            t.IsSend == request.IsSend &&
                            (string.IsNullOrWhiteSpace(request.Name) || t.Title.ToLower().Contains(request.Name.ToLower())) &&
                            (request.IncidentTypeIds.Count == 0 || request.IncidentTypeIds.Contains(t.Id)) &&
                            (request.TrainStationIds.Count == 0 || request.TrainStationIds.Contains(t.Id)) &&
                            (request.IndcidentIds.Count == 0 || request.IndcidentIds.Contains(t.Id)) &&
                            ((request.DateFrom == default(DateTime) || (request.DateTo == default(DateTime)) ||
                             (t.SendingDateTime >= request.DateFrom && t.SendingDateTime <= request.DateTo))))
                .OrderByDescending(t => t.Created)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .AsNoTracking()
                .ToListAsync();

            return incidents;
        }

        public Task<int> Count(bool isSent)
            => _incidents.Where(t => t.IsSend == isSent && t.IsDeleted == false).CountAsync();

    }
}
