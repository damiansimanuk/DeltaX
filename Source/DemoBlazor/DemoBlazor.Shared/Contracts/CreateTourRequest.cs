namespace DemoBlazor.Shared.Contracts;
using DemoBlazor.Shared.Entities;
using DeltaX.ResultFluent;
using MediatR;
using DeltaX.Core.Common;

public record CreateTourRequest(
    bool Failure,
    bool Exception,
    string Name,
    string Description)
    : IRequest<Result<TourDto>>;