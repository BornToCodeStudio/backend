using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using borntocode_backend.Database.Models;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace borntocode_backend.Services;

public class TokenService
{
    public string CreateToken(User user, Role role)
    {
        var jwtHandler = new JwtSecurityTokenHandler();

        var validIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
        var validAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
        if (string.IsNullOrWhiteSpace(validIssuer) || string.IsNullOrWhiteSpace(validAudience))
            throw new NullReferenceException();

        var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY")!);
        
        var expires = DateTime.UtcNow.AddMinutes(30);
        var issuedAt = DateTime.UtcNow;
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), 
            SecurityAlgorithms.HmacSha256);

        var claims = new Dictionary<string, object>
        {
            { ClaimTypes.NameIdentifier, user.Name },
            { ClaimTypes.Role, role.Name }
        };

        var descriptor = new SecurityTokenDescriptor
        {
            Audience = validAudience,
            Issuer = validIssuer,
            Expires = expires,
            IssuedAt = issuedAt,
            TokenType = "JWT",
            SigningCredentials = signingCredentials,
            Claims = claims
        };
        
        var token = jwtHandler.CreateToken(descriptor);

        return jwtHandler.WriteToken(token);
    }

    public bool ValidateToken(string token)
    {
        var jwtHandler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY")!);
        
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
            ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
            IssuerSigningKey = new SymmetricSecurityKey(key),
        };

        jwtHandler.ValidateToken(token, validationParameters, out var result);

        return result != null;
    }
}