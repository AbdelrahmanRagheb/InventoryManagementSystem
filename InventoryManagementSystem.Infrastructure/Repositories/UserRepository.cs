using InventoryManagementSystem.Application.Repositories;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Infrastructure.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public override async Task<User?> GetByIdAsync(Guid id) =>
        await _context.Set<User>().Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id);

    public override async Task<IReadOnlyList<User>> GetAllAsync() =>
        await _context.Set<User>().Include(u => u.Role).ToListAsync();

    public async Task UpdateAsync(User entity)
    {
        _context.Set<User>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<User?> GetByUsernameAsync(string username) =>
        await _context.Set<User>().Include(u => u.Role).FirstOrDefaultAsync(u => u.Username == username);

    public async Task<User?> GetByEmailAsync(string email) =>
        await _context.Set<User>().Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == email);

    public async Task<bool> UsernameExistsAsync(string username) =>
        await _context.Set<User>().AnyAsync(u => u.Username == username);

    public async Task<bool> EmailExistsAsync(string email) =>
        await _context.Set<User>().AnyAsync(u => u.Email == email);
}