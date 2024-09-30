namespace DeltaX.Core.Hosting.Auth;
using DeltaX.ResultFluent;
using Microsoft.Extensions.Configuration;
using System.Linq;

public class AuthorizationService(ICurrentUser currentUser, IConfiguration configuration)
{
    public ICurrentUser CurrentUser => currentUser;

    public bool SkipAuthorization { get; private set; } = configuration.GetValue("SkipAuthorization", false);

    private Result<ResultSuccess> ValidateAccessFilteredInternal(string? filteredPermission, params string[] permissions)
    {
        if (SkipAuthorization)
        {
            return Result.ResultSuccess;
        }

        if (!currentUser.IsAuthenticated)
        {
            Error.Unauthorized("User is not authenticated.");
        }

        if (filteredPermission is null && !permissions.Any())
        {
            return Result.ResultSuccess;
        }

        var claims = currentUser.Principal.FindAll(c => c.Type == "permission" || c.Type == "permissions");
        claims = filteredPermission is null ? claims : claims.Where(c => c.Value.Contains(filteredPermission));
        var hasPermissions = claims.Any(c => permissions.Any(v => c.Value.Contains(v)));

        if (hasPermissions)
        {
            return Result.ResultSuccess;
        }

        return Error.Forbidden("You do not have sufficient permissions to perform this operation.");
    }

    public Result<ResultSuccess> ValidateAccess(params string[] permissions)
    {
        return ValidateAccessFilteredInternal(null, permissions);
    }

    public Result<ResultSuccess> ValidateAccessFiltered(string filteredPermission, params string[] permissions)
    {
        return ValidateAccessFilteredInternal(filteredPermission, permissions);
    }


    public Result<ResultSuccess> ValidateAccessAction(string? resource, params string[] actions)
    {
        resource = string.IsNullOrWhiteSpace(resource) ? null : $"resource:{resource}";
        return ValidateAccessFilteredInternal(resource, actions);
    }
}