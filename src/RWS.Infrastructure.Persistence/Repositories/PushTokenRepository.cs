using RWS.Application.Interfaces.Repositories;
using RWS.Domain.Entities;
using RWS.Infrastructure.Persistence.Contexts;
using RWS.Infrastructure.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace RWS.Infrastructure.Persistence.Repositories
{
    public class PushTokenRepository : GenericRepository<PushToken, Guid>, IPushTokenRepository
    {
        private readonly DbSet<PushToken> _pushTokens;

        public PushTokenRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _pushTokens = dbContext.Set<PushToken>();
        }

        public async Task<PushToken> GetByUserId(Guid userId)
        {
            var token = await _pushTokens
                .Where(t => t.IsDeleted == false &&
                            t.UserId == userId)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return token;
        }
    }
}
