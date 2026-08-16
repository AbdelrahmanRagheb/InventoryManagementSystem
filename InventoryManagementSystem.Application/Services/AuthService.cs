using InventoryManagementSystem.Application.Authentication;
using InventoryManagementSystem.Application.Repositories;
using InventoryManagementSystem.Domain.Entities;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepo;
    private readonly IPasswordHasher _hasher;
    private readonly ITokenService _tokenService;
    public AuthService(IUserRepository userRepo, IPasswordHasher hasher, ITokenService tokenService)
    {
        _userRepo = userRepo; _hasher = hasher; _tokenService = tokenService;
    }

    public async Task<(bool Success, string? Token, string? Error)> Login(string username, string password)
    {
        var user = await _userRepo.GetByUsernameAsync(username);
        if (user == null) return (false, null, "Invalid username or password");
        if (!_hasher.Verify(password, user.PasswordHash)) return (false, null, "Invalid username or password");
        var token = _tokenService.GenerateToken(user.Id.ToString(), user.Username, user.Role);
        return (true, token, null);
    }

    public async Task<(bool Success, string? Error)> Register(string username, string email, string password, string role)
    {
        var existingUser = await _userRepo.GetByUsernameAsync(username);
        if (existingUser != null) return (false, "Username already exists");
        var existingEmail = await _userRepo.GetByEmailAsync(email);
        if (existingEmail != null) return (false, "Email already exists");
        var hashed = _hasher.Hash(password);
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = username,
            Email = email,
            PasswordHash = hashed,
            Role = role,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        await _userRepo.AddAsync(user);
        return (true, null);
    }
}