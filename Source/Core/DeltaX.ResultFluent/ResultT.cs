namespace DeltaX.ResultFluent;


public class Result<T> : Result
{
    public T? Value { get; init; }

    public Result() { }

    public Result(T? value, params Error[] errors) : base(errors)
    {
        Value = value;
    }

    public static implicit operator Result<T>(T v) => Result.Success(v);
    public static implicit operator Result<T>(Error e) => Result.Failure<T>(e);
    public static implicit operator Result<T>(Error[] errors) => Result.Failure<T>(errors);
    public static implicit operator Result<T>(Exception e) => Result.Exception<T>(e);
}
