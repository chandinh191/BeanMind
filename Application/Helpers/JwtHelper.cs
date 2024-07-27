using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Domain.Constants;
using System.Data;
using Application.Users;

namespace Application.Helpers;

public static class JwtHelper
{
    public const int refreshTokenLength = 20;

    public static string generateAccessToken(ApplicationUser user, IConfiguration configuration, UserManager<ApplicationUser> _userManager)
    {
        // required environment variable data
        var jwtIssuer = configuration.GetValue<string>("Jwt:Issuer");
        var jwtAudience = configuration.GetValue<string>("Jwt:Audience");
        var jwtSecretKey = configuration.GetValue<string>("Jwt:SecretKey");
        var jwtExpiresTime = configuration.GetValue<int>("Jwt:ExpiresIn");
        var roles = _userManager.GetRolesAsync(user).Result;

        var tokenHandler = new JwtSecurityTokenHandler();

        // claim identity
        var claims = new ClaimsIdentity(new[] {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, "AccessToken"),
            new Claim(ClaimTypes.Email, user.Email),
            //new Claim(ClaimTypes.Role, roles.FirstOrDefault() ?? string.Empty)
        });

        // Add all role 
        foreach (var role in roles)
        {
            claims.AddClaim(new Claim(ClaimTypes.Role, role));
        }

        // encoding key and create sign in credential
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey));
        var signInCredential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Subject = claims,
            Issuer = jwtIssuer,
            Audience = jwtAudience,
            Expires = DateTime.UtcNow.AddDays(jwtExpiresTime),
            SigningCredentials = signInCredential
        });

        return tokenHandler.WriteToken(token);
    }

    public static string generateRefreshToken(ApplicationUser user, IConfiguration configuration, UserManager<ApplicationUser> _userManager)
    {
        // required environment variable data
        var jwtIssuer = configuration.GetValue<string>("Jwt:Issuer");
        var jwtAudience = configuration.GetValue<string>("Jwt:Audience");
        var jwtSecretKey = configuration.GetValue<string>("Jwt:SecretKey");
        var jwtExpiresTime = configuration.GetValue<int>("Jwt:ExpiresIn");
        var roles = _userManager.GetRolesAsync(user).Result;

        var tokenHandler = new JwtSecurityTokenHandler();

        // claim identity
        var claims = new ClaimsIdentity(new[] {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, "RefreshToken"),
            new Claim(ClaimTypes.Email, user.Email),
            //new Claim(ClaimTypes.Role, roles.FirstOrDefault() ?? string.Empty)  
        });

        // Add all role 
        foreach (var role in roles)
        {
            claims.AddClaim(new Claim(ClaimTypes.Role, role));
        }

        // encoding key and create sign in credential
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey));
        var signInCredential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Subject = claims,
            Issuer = jwtIssuer,
            Audience = jwtAudience,
            Expires = DateTime.UtcNow.AddDays(jwtExpiresTime),
            SigningCredentials = signInCredential
        });

        return tokenHandler.WriteToken(token);
    }
}
