namespace AuthService.Api.Contracts;

public record RegisterRequest(string Username, string Email, string Password, string? Role);
public record LoginRequest(string UsernameOrEmail, string Password);
public record RefreshRequest(string RefreshToken);
public record AuthResponse(string Token, string RefreshToken, string Username, string Email, string Role);
public record UserResponse(int ID, string Username, string Email, string Role, DateTime CreatedAt);
public record UpdateRoleRequest(string Role);
public record UpdateUsernameRequest(string Username);
public record UpdateEmailRequest(string Email, string Password);
public record UpdatePasswordRequest(string CurrentPassword, string NewPassword);
