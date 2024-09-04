namespace DeltaX.ResultFluent;

public static class ResultExtensions
{
    public static Result<M> Bind<T, M>(this Result<T> result, Func<T, Result<M>> func)
    {
        return result.IsError
            ? Result.Failure<M>(result.Errors)
            : Result.Success(() => func(result.Value!));
    }

    public static Result<M> Bind<T, M>(this Result<T> result, Func<T, M> func)
    {
        return result.IsError
            ? Result.Failure<M>(result.Errors)
            : Result.Success(() => func(result.Value!));
    }

    public static Task<Result<M>> BindAsync<T, M>(this Task<Result<T>> result, Func<T, Task<Result<M>>> func)
    {
        return Result.SuccessAsync(async () =>
        {
            var res = await Result.SuccessAsync(result);
            return res.IsSuccess
                ? await func(res.Value!)
                : Result.Failure<M>(res.Errors);
        });
    }

    public static Task<Result<M>> BindAsync<T, M>(this Task<Result<T>> result, Func<T, Task<M>> func)
    {
        return Result.SuccessAsync(async () =>
        {
            var res = await Result.SuccessAsync(result);
            return res.IsSuccess
                ? Result.Success(await func(res.Value!))
                : Result.Failure<M>(res.Errors);
        });
    }

    public static Task<Result<M>> BindAsync<T, M>(this Result<T> result, Func<T, Task<Result<M>>> func)
    {
        return Result.SuccessAsync(async () =>
        {
            return result.IsSuccess
                ? await func(result.Value!)
                : Result.Failure<M>(result.Errors);
        });
    }

    public static Task<Result<M>> BindAsync<T, M>(this Result<T> result, Func<T, Task<M>> func)
    {
        return Result.SuccessAsync(async () =>
        {
            return result.IsSuccess
                ? Result.Success(await func(result.Value!))
                : Result.Failure<M>(result.Errors);
        });
    }

    public static async Task<Result<M>> BindAsync<M>(this Result<ResultSuccess> result, Func<Task<M>> func)
    {
        return result.IsSuccess
            ? await Result.SuccessAsync(func)
            : Result.Failure<M>(result.Errors);
    }

    public static Result<M> Map<T, M>(this Result<T> result, Func<T, M> func)
    {
        return result.IsError
           ? Result.Failure<M>(result.Errors)
           : Result.Success(() => func(result.Value!));
    }

    public static Result<T> Then<T>(this Result<T> result, Action<T> func)
    {
        return result.IsError ? result : Result.Success(() =>
        {
            func(result.Value!);
            return result;
        });
    }

    public static M Match<T, M>(this Result<T> result, Func<T, M> success, Func<IReadOnlyList<Error>, M> failure)
    {
        return result.IsSuccess
            ? success(result.Value!)
            : failure(result.Errors);
    }

    public static void Switch<T>(this Result<T> result, Action<T> success, Action<IReadOnlyList<Error>> failure)
    {
        if (result.IsSuccess)
        {
            success(result.Value!);
        }
        else
        {
            failure(result.Errors);
        }
    }
}
