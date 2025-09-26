namespace AuthService.Api.Contracts;

public record RegisterRequest(string Username, string Email, string Password, string Role);
public record LoginRequest(string UsernameOrEmail, string Password);
public record AuthResponse(string AccessToken, DateTime ExpiresAt,
                           int Id, string Username, string Email, string Role);
