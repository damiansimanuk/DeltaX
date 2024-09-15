namespace DemoBlazor.Handlers;
using DeltaX.Core.Hosting.Auth;
using DeltaX.ResultFluent;
using DemoBlazor.Database;
using DemoBlazor.Database.Entities;
using DemoBlazor.Services;
using DemoBlazor.Shared.Contracts;
using DemoBlazor.Shared.Entities;
using DemoBlazor.Shared.Events;
using MediatR;

public class CreateTourHandler(
    DemoBlazorDbContext dbContext,
    AuthorizationService authorization,
    MapperService mapper
    ) : IRequestHandler<CreateTourRequest, Result<TourDto>>
{
    public async Task<Result<TourDto>> Handle(CreateTourRequest request, CancellationToken cancellationToken)
    {
        if (request.Exception)
        {
            throw new InvalidOperationException("Excepcion genearad por pedido del cliente!");
        }

        var eb = ErrorBuilder.Create()
            .Add(authorization.ValidateAccess(nameof(CreateTourRequest)))
            .Add(request.Failure, Error.Create("HighSeverity", $"Request processing failed due to: {request.Failure}"))
            .Add(string.IsNullOrWhiteSpace(request.Name), Error.InvalidArgument("The 'Name' field cannot be empty."))
            .Add(string.IsNullOrWhiteSpace(request.Description), Error.InvalidArgument("The 'Description' field cannot be empty."));

        if (eb.HasError)
        {
            return eb.GetErrors();
        }

        var tour = new Tour
        {
            Name = request.Name,
            UserId = authorization.CurrentUser.Identifier,
            Description = request.Description,
        };

        dbContext.AddDomainEvent(() => new TourCreated(tour.Id, tour.Name, tour.Description));
        dbContext.Add(tour);

        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.ToDto(tour);
    }
}

