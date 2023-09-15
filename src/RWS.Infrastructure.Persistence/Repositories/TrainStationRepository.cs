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
    public class TrainStationRepository : GenericRepository<TrainStation, Guid>, ITrainStationRepository
    {
        private readonly DbSet<TrainStation> _trainStations;

        public TrainStationRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _trainStations = dbContext.Set<TrainStation>();
        }

        public Task<bool> Exists(Guid id)
            => _trainStations.Where(t => t.Id == id && t.IsDeleted == false).AnyAsync();

        public Task<int> Count()
            => _trainStations.Where(t => t.IsDeleted == false).CountAsync();

        public async Task<IReadOnlyList<TrainStation>> GetByNameFilterAsync(string searchingString, int pageNumber, int pageSize)
        {
            // Небольшой костыль для решения недоработок EF
            List<string> sub = new List<string>();
            var s = searchingString != null ? searchingString.Split(" ") : null;

            for (int i = 0; i < 5; i++)
                if ((s != null) && (i < s.Length) && (s[i].Length > 0))
                    sub.Add(s[i]);
                else
                    sub.Add("");

            return
                await _trainStations
                //.Where(t => t.Name.Contains(searchingString))
                .Where(t => sub[0].Length == 0 || t.Name.Contains(sub[0]))
                .Where(t => sub[1].Length == 0 || t.Name.Contains(sub[1]))
                .Where(t => sub[2].Length == 0 || t.Name.Contains(sub[2]))
                .Where(t => sub[3].Length == 0 || t.Name.Contains(sub[3]))
                .Where(t => sub[4].Length == 0 || t.Name.Contains(sub[4]))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

        }
    }
}
