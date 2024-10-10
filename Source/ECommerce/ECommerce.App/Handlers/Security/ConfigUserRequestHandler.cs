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
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

public class ConfigUserRequestHandler(
    SecurityDbContext dbContext,
    AuthorizationService authorization,
    IUserStore<User> userStore,
    UserManager<User> userManager
) : IRequestHandler<ConfigUserRequest, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(ConfigUserRequest request, CancellationToken cancellationToken)
    {
        var eb = ErrorBuilder.Create()
            // .Add(authorization.ValidateAccess(nameof(ConfigRoleRequest)))
            .Add(string.IsNullOrWhiteSpace(request.Email), Error.InvalidArgument("The 'Email' field cannot be empty."))
            .Add(string.IsNullOrWhiteSpace(request.FullName), Error.InvalidArgument("The 'FullName' field cannot be empty."))
            .Add(request.Roles == null, Error.InvalidArgument("The 'Roles' field cannot be null."));

        if (eb.HasError)
        {
            return eb.GetErrors();
        }

        var email = userManager.NormalizeEmail(request.Email);
        var user = dbContext.Set<User>().Include(e => e.Roles).FirstOrDefault(u => u.NormalizedEmail == email);
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

        await userStore.SetUserNameAsync(user, request.Email, cancellationToken);
        var emailStore = (IUserEmailStore<User>)userStore;
        await emailStore.SetEmailAsync(user, request.Email, cancellationToken);

        user.FullName = request.FullName?.Normalize() ?? user.FullName;
        user.EmailConfirmed = true;
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