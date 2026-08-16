using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using InventoryManagementSystem.Application.Authentication;
using InventoryManagementSystem.Domain.Entities;

namespace InventoryManagementSystem.Infrastructure.Authentication;

public class JwtTokenService : ITokenService
{
    private readonly JwtSettings _settings;

    public JwtTokenService(JwtSettings settings) => _settings = settings;

    public string GenerateToken(string userId, string username, string role)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimsIdentity.DefaultNameClaimType, username),
            new Claim(ClaimTypes.Role, role)
        };

        var key = Encoding.UTF8.GetBytes(_settings.SecretKey!);
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_settings.ExpiryMinutes),
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}