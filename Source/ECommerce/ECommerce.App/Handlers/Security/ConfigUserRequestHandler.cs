namespace ECommerce.App.Handlers.Security;

using DeltaX.Core.Hosting.Auth;
using DeltaX.ResultFluent;
using ECommerce.App.Database;
using ECommerce.App.Database.Entities.Security;
using ECommerce.App.Services;
using ECommerce.Shared.Contracts.Security;
using ECommerce.Shared.Entities.Security;
using Humanizer;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

public class ConfigUserRequestHandler(
    SecurityDbContext dbContext,
    AuthorizationService authorization
) : IRequestHandler<ConfigUserRequest, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(ConfigUserRequest request, CancellationToken cancellationToken)
    {
        var eb = ErrorBuilder.Create()
            // .Add(authorization.ValidateAccess(nameof(ConfigRoleRequest)))
            .Add(string.IsNullOrWhiteSpace(request.UserName), Error.InvalidArgument("The 'UserName' field cannot be empty."))
            .Add(string.IsNullOrWhiteSpace(request.Email), Error.InvalidArgument("The 'Email' field cannot be empty."))
            .Add(request.Roles == null, Error.InvalidArgument("The 'Roles' field cannot be null."));

        if (eb.HasError)
        {
            return eb.GetErrors();
        }

        var user = dbContext.Set<User>().Include(u => u.Roles).FirstOrDefault(e => e.Email == request.Email);
        if (user == null)
        {
            return Error.InvalidArgument("The 'Email' is not registered.");
        }

        var roles = dbContext.Set<Role>()
            .Where(e => request.Roles!.Contains(e.Name))
            .ToList();
        var existentRoles = roles
            .Where(e => !user.Roles.Any(r => r.Name == e.Name))
            .ToList();
        var newRoles = request.Roles!
            .Where(e => !roles.Any(c => c.Name == e))
            .Select(e => new Role { Name = e, NormalizedName = e.Normalize() })
            .ToList();

        user.UserName = request.UserName ?? user.UserName;
        user.NormalizedUserName = request.UserName?.Normalize() ?? user.NormalizedUserName;
        user.FullName = request.FullName?.Normalize() ?? user.FullName;
        user.Email = request.Email ?? user.Email;
        user.NormalizedEmail = request.Email?.ToUpper() ?? user.NormalizedEmail;
        user.EmailConfirmed = !string.IsNullOrWhiteSpace(request.Email) || user.EmailConfirmed;
        user.PhoneNumber = request.PhoneNumber ?? user.PhoneNumber;

        user.Roles.AddRange(existentRoles);
        user.Roles.AddRange(newRoles);
        user.Roles.RemoveAll(r => !request.Roles!.Contains(r.Name));

        dbContext.Update(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new UserDto(
            user.Id,
            user.UserName,
            user.FullName,
            user.Email!,
            user.PhoneNumber,
            user.Roles.Select(r => r.Name).ToArray()!
            );
    }
}