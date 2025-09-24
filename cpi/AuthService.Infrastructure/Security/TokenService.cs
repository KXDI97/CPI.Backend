using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AuthService.Domain;

namespace AuthService.Infrastructure.Security;  

public class TokenService
{
    private readonly string _issuer, _audience, _key;
    public TokenService(IConfiguration cfg)
    {
        _issuer = cfg["Jwt:Issuer"]!;
        _audience = cfg["Jwt:Audience"]!;
        _key = cfg["Jwt:Key"]!;
    }

    public string Create(User u, TimeSpan? lifetime = null)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, u.ID.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, u.Username),
            new Claim(JwtRegisteredClaimNames.Email, u.Email),
            new Claim(ClaimTypes.Role, u.Role)
        };

        var creds = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key)),
                                           SecurityAlgorithms.HmacSha256);
        var now = DateTime.UtcNow;
        var token = new JwtSecurityToken(_issuer, _audience, claims,
            notBefore: now, expires: now.Add(lifetime ?? TimeSpan.FromHours(8)), signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
