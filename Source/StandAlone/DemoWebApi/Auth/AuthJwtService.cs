namespace DemoWebApi.Auth;

using DemoWebApi.Database;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public record JwtLoginResponse(string TokenType, string AccessToken, DateTimeOffset ExpiresIn);

public class AuthJwtService(IConfiguration configuration, AuthRepository authRepository)
{
    static TimeSpan expireTime = TimeSpan.FromMinutes(30);

    public int? GetUserIdIfValidCredential(string email, string password)
    {
        var userRecord = authRepository.GetUserRecord(null, email);
        if (userRecord != null && SecretHash.Verify(password, userRecord.PasswordHash))
        {
            return userRecord.UserId;
        }
        return null;
    }

    public (JwtLoginResponse?, ClaimsPrincipal?) Login(string email, string password)
    {
        var key = configuration.GetValue<string>("Jwt:Key")!;
        var issuer = configuration.GetValue<string>("Jwt:ValidIssuer")!;
        var audience = configuration.GetValue<string>("Jwt:ValidAudience")!;

        var userId = GetUserIdIfValidCredential(email, password);
        if (userId == null)
        {
            return (null, null);
        }

        var user = authRepository.GetUserInfo(userId.Value)!;

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
        };

        foreach (var r in user.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, r.Name));

            if (r.Permissions.Any())
            {
                var permissions = string.Join(", ", r.Permissions);
                claims.Add(new Claim("permission", $"role:{r.Name} {permissions}"));
            }
        }

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        var expireIn = DateTimeOffset.Now + expireTime;
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims,
            expires: expireIn.DateTime,
            signingCredentials: credentials);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

        return (new JwtLoginResponse("Bearer", accessToken, expireIn), principal);
    }
}
