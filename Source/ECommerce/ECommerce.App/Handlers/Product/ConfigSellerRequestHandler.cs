namespace ECommerce.App.Handlers.Product;
using DeltaX.Core.Hosting.Auth;
using DeltaX.ResultFluent;
using ECommerce.App.Database;
using ECommerce.App.Database.Entities.Product;
using ECommerce.App.Database.Entities.Security;
using ECommerce.App.Services;
using ECommerce.Shared.Contracts.Product;
using ECommerce.Shared.Entities.Product;
using ECommerce.Shared.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

public class ConfigSellerRequestHandler(
    ECommerceDbContext dbContext,
    AuthorizationService authorization,
    MapperService mapper
    ) : IRequestHandler<ConfigSellerRequest, Result<SellerDto>>
{
    public async Task<Result<SellerDto>> Handle(ConfigSellerRequest request, CancellationToken cancellationToken)
    {
        var eb = ErrorBuilder.Create()
             //  .Add(authorization.ValidateAccess(nameof(ConfigSellerRequest)))
             .Add(string.IsNullOrWhiteSpace(request.Name), Error.InvalidArgument("The 'Name' field cannot be empty."))
             .Add(string.IsNullOrWhiteSpace(request.Email), Error.InvalidArgument("The 'Email' field cannot be empty."))
             .Add(string.IsNullOrWhiteSpace(request.PhoneNumber), Error.InvalidArgument("The 'PhoneNumber' field cannot be empty."));

        if (eb.HasError)
        {
            return eb.GetErrors();
        }

        var user = dbContext.Set<User>().FirstOrDefault(e => e.Id == authorization.CurrentUser.Identifier);
        var seller = dbContext.Set<Seller>().Include(s => s.Users).FirstOrDefault(e => e.Id == request.SellerId);

        if (seller == null)
        {
            seller = new Seller
            {
                Name = request.Name,
                Email = request.Email.Normalize().ToLower(),
                PhoneNumber = request.PhoneNumber,
            };
            dbContext.Add(seller);
        }
        else
        {
            seller.Name = request.Name;
            seller.Email = request.Email.Normalize().ToLower();
            seller.PhoneNumber = request.PhoneNumber;
            dbContext.Update(seller);
        }

        if (user != null)
        {
            seller.Users.Add(user);
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.ToDto(seller);
    }
}
