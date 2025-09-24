namespace AuthService.Domain;

public class User
{
    public int ID { get; set; }
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public string Role { get; set; } = "Viewer";
    public byte[]? PasswordHash { get; set; }
    public byte[]? PasswordSalt { get; set; }
}
