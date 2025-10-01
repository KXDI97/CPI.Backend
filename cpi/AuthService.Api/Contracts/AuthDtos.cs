namespace AuthService.Api.Contracts;

public record RegisterRequest(string Username, string Email, string Password, string Role);
public record LoginRequest(string UsernameOrEmail, string Password);
public record AuthResponse(string Token, string Username, string Email, string Role);
public record UserResponse(int ID, string Username, string Email, string Role);
