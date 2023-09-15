using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWS.Application.Interfaces.Repositories.Common
{
    public interface IGenericRepository<EntityType, KeyType>
    {
        Task<EntityType> GetByIdAsync(KeyType id);
        Task<IReadOnlyList<EntityType>> GetAllAsync();
        Task<IReadOnlyList<EntityType>> GetAllPagedResponseAsync(int pageNumber, int pageSize);
        Task<EntityType> AddAsync(EntityType entity);
        Task UpdateAsync(EntityType entity);
        Task DeleteAsync(EntityType entity);
    }
}
