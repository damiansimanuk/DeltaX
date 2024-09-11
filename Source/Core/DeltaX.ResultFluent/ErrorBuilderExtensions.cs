namespace DeltaX.ResultFluent;

public static class ErrorBuilderExtensions
{
    public static ErrorBuilder Validate(this ErrorBuilder builder, Func<bool> validator, Func<Error> error)
    {
        return !validator() ? builder.Add(error()) : builder;
    }

    public static ErrorBuilder Add(this ErrorBuilder builder, bool isError, Func<Error> error)
    {
        return isError ? builder.Add(error()) : builder;
    }

    public static ErrorBuilder Add(this ErrorBuilder builder, bool isError, params Error[] errors)
    {
        return isError ? builder.Add(errors) : builder;
    }

    public static ErrorBuilder Add(this ErrorBuilder builder, Result<ResultSuccess> validation)
    {
        return validation.IsError ? builder.Add(validation.Errors) : builder;
    }

    public static Result<ResultSuccess> ToResult(this ErrorBuilder builder)
    {
        return builder.HasError ? builder.GetErrors() : Result.ResultSuccess;
    }

    public static Result<T> ToResult<T>(this ErrorBuilder builder, T value)
    {
        return builder.HasError ? builder.GetErrors() : Result.Success(value);
    }

    public static Result<T> ToResult<T>(this ErrorBuilder builder, Func<T> func)
    {
        return builder.HasError ? builder.GetErrors() : Result.Success(func);
    }

    public static Result<T> ToResult<T>(this ErrorBuilder builder, Func<Result<T>> func)
    {
        return builder.HasError ? builder.GetErrors() : Result.Success(func);
    }

    public static async Task<Result<T>> ToResultAsync<T>(this ErrorBuilder builder, Func<Task<T>> func)
    {
        return builder.HasError ? builder.GetErrors() : await Result.SuccessAsync(func);
    }

    public static async Task<Result<T>> ToResultAsync<T>(this ErrorBuilder builder, Func<Task<Result<T>>> func)
    {
        return builder.HasError ? builder.GetErrors() : await Result.SuccessAsync(func);
    }
}
