using InventoryManagementSystem.Application.Authentication;
using InventoryManagementSystem.Application.Repositories;
using InventoryManagementSystem.Domain.Entities;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repo;
    private readonly IPasswordHasher _hasher;
    private readonly ITokenService _tokenService;
    public UserService(IUserRepository repo, IPasswordHasher hasher, ITokenService tokenService)
    {
        _repo = repo; _hasher = hasher; _tokenService = tokenService;
    }

    public async Task<IReadOnlyList<User>> GetAllAsync() => await _repo.GetAllAsync();
    public async Task<User?> GetByIdAsync(Guid id) => await _repo.GetByIdAsync(id);
    public async Task<User?> GetByUsernameAsync(string username) => await _repo.GetByUsernameAsync(username);

    public async Task UpdateAsync(Guid id, string? username = null, string? email = null, string? displayName = null, bool? isActive = null)
    {
        var user = await _repo.GetByIdAsync(id);
        if (user == null) return;
        if (!string.IsNullOrEmpty(username)) user.Username = username;
        if (!string.IsNullOrEmpty(email)) user.Email = email;
        if (displayName != null) user.DisplayName = displayName;
        if (isActive.HasValue) user.IsActive = isActive.Value;
        await _repo.UpdateAsync(user);
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await _repo.GetByIdAsync(id);
        if (user != null) { user.IsActive = false; await _repo.UpdateAsync(user); }
    }

    public async Task<(bool Success, string? Error)> ChangePasswordAsync(Guid userId, string oldPassword, string newPassword)
    {
        var user = await _repo.GetByIdAsync(userId);
        if (user == null) return (false, "User not found");
        if (!_hasher.Verify(oldPassword, user.PasswordHash)) return (false, "Old password incorrect");
        user.PasswordHash = _hasher.Hash(newPassword);
        await _repo.UpdateAsync(user);
        return (true, null);
    }
}