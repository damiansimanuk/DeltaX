namespace ECommerce.App.Services;

using ECommerce.App.Database;
using ECommerce.App.Database.Entities.Security;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

public class ApplicationClaimsFactory(
        IServiceProvider serviceProvider
    ) : IUserClaimsPrincipalFactory<User>
{
    public Task<ClaimsPrincipal> CreateAsync(User user)
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
        var customClaims = new[] { "resource", "action", "permission", "permissions" };

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

    /// <summary>
    /// return claim like `resource:machineId:1 Justify Edit` 
    /// </summary>
    /// <param name="claims"></param>
    /// <returns></returns>
    private Claim? JoinPermissionClaims(IEnumerable<Claim> claims)
    {
        var customClaims = new[] { "action", "permission", "permissions" };
        var values = new List<string>();

        foreach (var resource in claims.Where(c => c.Type == "resource").Select(e => e.Value))
        {
            values.Add($"resource:{resource}");
        }

        var permissions = claims.Where(c => customClaims.Contains(c.Type)).SelectMany(e => e.Value.Split(' ')).ToList();
        if (permissions?.Any() == true)
        {
            values.AddRange(permissions.Where(p => !string.IsNullOrEmpty(p)));
        }

        return values.Any() ? new Claim("permissions", string.Join(' ', values)) : null;
    }
}
