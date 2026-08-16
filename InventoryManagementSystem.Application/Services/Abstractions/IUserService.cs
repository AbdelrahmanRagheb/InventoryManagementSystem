using InventoryManagementSystem.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Services;

public interface IUserService
{
    Task<IReadOnlyList<User>> GetAllAsync();
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByUsernameAsync(string username);
    Task UpdateAsync(Guid id, string? username = null, string? email = null, string? displayName = null, bool? isActive = null);
    Task DeleteAsync(Guid id);
    Task<(bool Success, string? Error)> ChangePasswordAsync(Guid userId, string oldPassword, string newPassword);
}