using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Repositories;

public interface IUnitOfWork
{
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}