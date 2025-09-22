namespace AuthService.Domain;

public class User
{
    public int ID { get; set; }                // PK
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public string Role { get; set; } = "Viewer"; // Admin | Seller | Viewer
    public byte[]? PasswordHash { get; set; }
    public byte[]? PasswordSalt { get; set; }
}
