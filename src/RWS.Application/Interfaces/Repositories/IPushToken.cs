using System;
using System.Threading.Tasks;
using RWS.Application.Interfaces.Repositories.Common;
using RWS.Domain.Entities;

namespace RWS.Application.Interfaces.Repositories
{
    public interface IPushTokenRepository : IGenericRepository<PushToken, Guid>
    {
        Task<PushToken> GetByUserId(Guid userId);
    }
}
