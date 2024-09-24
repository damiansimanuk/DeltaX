namespace ECommerce.App.Handlers;
using DeltaX.Core.Hosting.Auth;
using DeltaX.ResultFluent;
using ECommerce.App.Database;
using ECommerce.App.Database.Entities;
using ECommerce.App.Services;
using ECommerce.Shared.Contracts;
using ECommerce.Shared.Entities;
using ECommerce.Shared.Events;
using MediatR;

public class ConfigSellerRequestHandler(
    ECommerceDbContext dbContext,
    AuthorizationService authorization,
    MapperService mapper
    ) : IRequestHandler<ConfigSellerRequest, Result<SellerDto>>
{
    public async Task<Result<SellerDto>> Handle(ConfigSellerRequest request, CancellationToken cancellationToken)
    {
        var eb = ErrorBuilder.Create()
             .Add(authorization.ValidateAccess(nameof(ConfigSellerRequest)))
             .Add(string.IsNullOrWhiteSpace(request.Name), Error.InvalidArgument("The 'Name' field cannot be empty."))
             .Add(string.IsNullOrWhiteSpace(request.Email), Error.InvalidArgument("The 'Email' field cannot be empty."))
             .Add(string.IsNullOrWhiteSpace(request.PhoneNumber), Error.InvalidArgument("The 'PhoneNumber' field cannot be empty."));

        if (eb.HasError)
        {
            return eb.GetErrors();
        }

        var user = dbContext.Set<User>().FirstOrDefault(e => e.Id == authorization.CurrentUser.Identifier);

        var seller = new Seller
        {
            Name = request.Name,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Users = [user],
        };

        dbContext.Add(seller);
        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.ToDto(seller);
    }
}
