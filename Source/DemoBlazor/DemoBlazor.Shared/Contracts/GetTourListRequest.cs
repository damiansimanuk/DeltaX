namespace DemoBlazor.Shared.Contracts;
using DemoBlazor.Shared.Entities;
using DeltaX.ResultFluent;
using MediatR;
using DeltaX.Core.Common;

public record GetTourListRequest(
    string FilterText,
    Pagination Pagination)
    : IRequest<Pagination<TourDto>>;
