using InventoryManagementSystem.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Repositories;

public interface IResourceRepository : IRepository<Resource>
{
    Task UpdateAsync(Resource resource);
    Task<IReadOnlyList<Resource>> GetByTypeAsync(string type);
}