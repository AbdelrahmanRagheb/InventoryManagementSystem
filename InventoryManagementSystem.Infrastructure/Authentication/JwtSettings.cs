namespace InventoryManagementSystem.Infrastructure.Authentication;

public class JwtSettings
{
    public string Issuer { get; set; } = "InventoryManagementSystem";
    public string Audience { get; set; } = "InventoryClients";
    public string SecretKey { get; set; } = "a_very_secret_key_at_least_32_chars!";
    public int ExpiryMinutes { get; set; } = 60;
}