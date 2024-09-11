namespace DemoBlazor.Shared.Contracts;
using DemoBlazor.Shared.Entities;
using DeltaX.ResultFluent;
using MediatR;

public record CreateTourRequest(
    bool Failure,
    bool Exception,
    string Name,
    string Description)
    : IRequest<Result<TourDto>>;

public class Service
{
    Result<TourDto> CreateRequest(
        bool failure,
        bool exception,
        string name,
        string description) => throw new NotImplementedException();
}