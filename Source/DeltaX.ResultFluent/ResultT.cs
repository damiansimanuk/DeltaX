namespace DeltaX.ResultFluent;

public class Result<T>
{
    public T? Value { get; init; }
    public Error[] Errors { get; init; } = [];
    public Error? FirstError => IsError ? Errors[0] : null;
    public bool IsSuccess { get; init; }
    public bool IsError { get; init; }

    public Result() { }

    public Result(T? value, params Error[] errors)
    {
        if (errors is not null && errors.Length > 0)
        {
            IsError = true;
            Errors = errors.ToArray();
        }
        else
        {
            IsSuccess = true;
            Value = value;
        }
    }

    public static implicit operator Result<T>(T v) => Result.Success(v);
    public static implicit operator Result<T>(Error e) => Result.Failure<T>(e);
    public static implicit operator Result<T>(Error[] errors) => Result.Failure<T>(errors);
    public static implicit operator Result<T>(Exception e) => Result.Exception<T>(e);
}
