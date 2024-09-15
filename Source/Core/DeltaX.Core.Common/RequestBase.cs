namespace DeltaX.Core.Common;
using DeltaX.ResultFluent;
using MediatR;
using System.Text.Json.Serialization;

public record RequestBase<T>() : IRequest<Result<T>>
{
    public Result<T>? Result { get; }
};
