namespace DemoBlazor.Services;

using DemoBlazor.Database;
using DemoBlazor.Database.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

public class ApplicationClaimsFactory(
        IServiceProvider serviceProvider
    ) : IUserClaimsPrincipalFactory<ApplicationUser>
{
    public Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
    {
        var claims = new List<Claim>{
            new Claim(ClaimTypes.Email, user.Email ?? ""),
            new Claim(ClaimTypes.Name, user.UserName?? ""),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim("permission", "line:1 CreateTourRequest"),
            //new Claim("TenantId",  ""),
        };

        claims.AddRange(GetClaims(user.Id));

        var claimsIdentity = new ClaimsIdentity(claims, "Bearer");

        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        return Task.FromResult(claimsPrincipal);

    }

    private List<Claim> GetClaims(string userId)
    {
        var result = new List<Claim>();

        using var scope = serviceProvider.CreateScope();
        using var securityDb = scope.ServiceProvider.GetRequiredService<SecurityDbContext>();

        var roleIds = securityDb.UserRoles.Where(r => r.UserId == userId).Select(ur => ur.RoleId).ToList();
        var roles = securityDb.Roles.Where(r => roleIds.Contains(r.Id)).ToList();
        var rolesClaims = securityDb.RoleClaims.Where(rc => roles.Select(r => r.Id).Contains(rc.RoleId)).ToList();
        var userClaims = securityDb.UserClaims.Where(rc => rc.UserId == userId).Select(e => e.ToClaim()).ToList();
        var customClaims = new[] { "line", "machines", "permissions" };

        result.AddRange(userClaims.Where(c => !customClaims.Contains(c.Type)));
        var permUserClaim = JoinPermissionClaims(userClaims);
        if (permUserClaim != null)
        {
            result.Add(permUserClaim);
        }

        foreach (var role in roles)
        {
            var roleClaims = rolesClaims.Where(e => e.RoleId == role.Id).Select(e => e.ToClaim()).ToList();
            result.Add(new Claim(ClaimTypes.Role, role.NormalizedName!));

            result.AddRange(roleClaims.Where(c => !customClaims.Contains(c.Type)));
            var permRoleClaim = JoinPermissionClaims(roleClaims);
            if (permRoleClaim != null)
            {
                result.Add(permRoleClaim);
            }
        }

        return result;
    }

    private Claim? JoinPermissionClaims(IEnumerable<Claim> claims)
    {
        var values = new List<string>();

        var line = claims.FirstOrDefault(c => c.Type == "line")?.Value;
        if (!string.IsNullOrEmpty(line))
        {
            values.Add($"l:{line}");
        }

        var machines = claims.FirstOrDefault(c => c.Type == "machines")?.Value?.Split(' ');
        if (machines?.Any() == true)
        {
            values.AddRange(machines.Where(m => !string.IsNullOrEmpty(m)).Select(m => $"m:{m}"));
        }

        var permissions = claims.FirstOrDefault(c => c.Type == "permissions")?.Value?.Split(' ');
        if (permissions?.Any() == true)
        {
            values.AddRange(permissions.Where(p => !string.IsNullOrEmpty(p)));
        }

        return values.Any() ? new Claim("permissions", string.Join(' ', values)) : null;
    }
}
