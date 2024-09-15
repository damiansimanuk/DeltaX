namespace DemoWebApi.Auth;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public record JwtLoginResponse(string TokenType, string AccessToken, DateTimeOffset ExpiresIn);
public record UserInfo(string UserName, string Email);

public class AuthJwtService(IConfiguration configuration)
{
    static TimeSpan expireTime = TimeSpan.FromMinutes(30);

    public JwtLoginResponse? Login(string email, string password, out ClaimsPrincipal? principal)
    {
        var key = configuration.GetValue<string>("Jwt:Key")!;
        var issuer = configuration.GetValue<string>("Jwt:ValidIssuer")!;

        // FAKE data
        var userId = Guid.NewGuid().ToString();
        var isLoggedIn = password != null;
        if (!isLoggedIn)
        {
            principal = null;
            return null;
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, email),
            new Claim(ClaimTypes.NameIdentifier, userId),
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        principal = new ClaimsPrincipal(identity);

        var expireIn = DateTimeOffset.Now + expireTime;
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new JwtSecurityToken(
            issuer: issuer,
            audience: issuer,
            claims,
            expires: expireIn.DateTime,
            signingCredentials: credentials);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

        return new JwtLoginResponse("Bearer", accessToken, expireIn);
    }
}
