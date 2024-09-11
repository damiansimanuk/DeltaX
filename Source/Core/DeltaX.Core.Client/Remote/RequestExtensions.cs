namespace DeltaX.Core.Client.Remote;
using DeltaX.ResultFluent;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

public static class RequestExtensions
{
    public static Task<Result<ResultSuccess>> WrapResponse(this Task<HttpResponseMessage> response)
    {
        return Result.SuccessAsync(async () =>
        {
            try
            {
                var res = await response.ConfigureAwait(false);
                if (res.IsSuccessStatusCode)
                {
                    return Result.ResultSuccess;
                }
                else
                {
                    return await ErrorResponse<ResultSuccess>(res);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        });
    }

    public static Task<Result<TResponse>> WrapResponse<TResponse>(this Task<HttpResponseMessage> response)
    {
        return Result.SuccessAsync<TResponse>(async () =>
        {
            try
            {
                var res = await response.ConfigureAwait(false);
                if (res.IsSuccessStatusCode)
                {
                    var content = res.StatusCode == HttpStatusCode.NoContent || res.Content.Headers.ContentLength == 0
                        ? default
                        : await res.Content.ReadFromJsonAsync<TResponse>().ConfigureAwait(false); ;
                    return Result.Success(content!);
                }
                else
                {
                    return await ErrorResponse<TResponse>(res);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        });
    }

    public static async Task<Result<TResponse>> ErrorResponse<TResponse>(HttpResponseMessage res)
    {
        var msgCode = $"Status: {(int)res.StatusCode} {res.StatusCode.ToString()}";
        var contentDetail = await res.Content.ReadAsStringAsync();

        switch (res.StatusCode)
        {
            case HttpStatusCode.NotFound:
                return Error.NotFound(res.ReasonPhrase ?? msgCode, contentDetail);
            case HttpStatusCode.Unauthorized:
                return Error.Unauthorized(res.ReasonPhrase ?? msgCode, contentDetail);
            case HttpStatusCode.Forbidden:
                return Error.Forbidden(res.ReasonPhrase ?? msgCode, contentDetail);
            default:
                return FormatError<TResponse>(msgCode, res.ReasonPhrase ?? msgCode, contentDetail);
        }
    }

    public static Result<TResponse> FormatError<TResponse>(string defaultCode, string defaultMessage, string contentDetail)
    {
        var json = JsonSerializer.Deserialize<JsonElement>(contentDetail);

        try
        {
            /*
              {
                  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                  "title": "One or more validation errors occurred.",
                  "status": 400,
                  "errors": {
                      "PasswordTooShort": [ "Passwords must be at least 6 characters." ],
                      "PasswordRequiresNonAlphanumeric": [ "Passwords must have at least one non alphanumeric character." ],
                      "PasswordRequiresDigit": [ "Passwords must have at least one digit ('0'-'9')." ],
                      "PasswordRequiresUpper": [ "Passwords must have at least one uppercase ('A'-'Z')." ]
                  }
              }
           */

            var eb = ErrorBuilder.Create();
            var errors = json.GetProperty("errors");
            if (errors.ValueKind == JsonValueKind.Object)
            {
                var res = errors.Deserialize<IDictionary<string, string[]>>()!;
                foreach (var kv in res)
                {
                    foreach (var v in kv.Value)
                    {
                        eb.Add(defaultCode, kv.Key, v);
                    }
                }
            }
            if (eb.HasError)
            {
                return eb.ToResult<TResponse>(null!);
            }
        }
        catch { }

        return Error.Create(defaultCode, defaultMessage, contentDetail);

    }

}
