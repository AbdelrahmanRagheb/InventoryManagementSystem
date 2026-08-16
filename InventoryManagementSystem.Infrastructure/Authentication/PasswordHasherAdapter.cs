using InventoryManagementSystem.Application.Authentication;
using InventoryManagementSystem.Infrastructure.Authentication;

namespace InventoryManagementSystem.Infrastructure.Authentication;

public class PasswordHasherAdapter : IPasswordHasher
{
    public string Hash(string password) => PasswordHasher.Hash(password);

    public bool Verify(string password, string hashedPassword) => PasswordHasher.Verify(password, hashedPassword);
}