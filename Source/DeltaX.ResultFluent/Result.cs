namespace DeltaX.ResultFluent;

public static class Result
{
    public static readonly ResultSuccess ResultSuccess = new();

    public static Result<T> Success<T>(Func<T> successProcessor)
    {
        try
        {
            return successProcessor();
        }
        catch (Exception e)
        {
            return Exception<T>(e);
        }
    }

    public static Result<T> Success<T>(Func<Result<T>> successProcessor)
    {
        try
        {
            return successProcessor();
        }
        catch (Exception e)
        {
            return Exception<T>(e);
        }
    }

    public static Result<T> Success<T>(Func<Task<Result<T>>> successProcessor)
    {
        return SuccessAsync(successProcessor).Result;
    }

    public static Result<T> Success<T>(Func<Task<T>> successProcessor)
    {
        return SuccessAsync(successProcessor).Result;
    }

    public static async Task<Result<T>> SuccessAsync<T>(Func<Task<Result<T>>> successProcessor)
    {
        try
        {
            return await successProcessor();
        }
        catch (Exception e)
        {
            return Exception<T>(e.InnerException ?? e);
        }
    }

    public static async Task<Result<T>> SuccessAsync<T>(Task<Result<T>> successProcessor)
    {
        try
        {
            return await successProcessor;
        }
        catch (Exception e)
        {
            return Exception<T>(e.InnerException ?? e);
        }
    }

    public static async Task<Result<T>> SuccessAsync<T>(Func<Task<T>> successProcessor)
    {
        try
        {
            return Success(await successProcessor());
        }
        catch (Exception e)
        {
            return Exception<T>(e.InnerException ?? e);
        }
    }

    public static Task<Result<T>> SuccessAsync<T>(Func<T> successProcessor)
    {
        return Task.FromResult(Success(successProcessor));
    }

    public static Result<T> Success<T>(T v) => new(v);

    public static Result<T> Error<T>(string code, string message, string? detail = null) => new(default, new Error(code, message, detail));

    public static Result<T> Failure<T>(params Error[] errors) => new(default!, errors);

    public static Result<T> Exception<T>(Exception e)
    {
        var code = e.GetType().Name;
        var error = new Error(code, e.Message, e.StackTrace);
        return new(default, error);
    }
}
