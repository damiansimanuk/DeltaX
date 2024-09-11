namespace DemoBlazor.Client.Shared;

using DeltaX.Core.Client.Remote;
using DeltaX.ResultFluent;
using MediatR;
using System.Collections.Specialized;
using System.Net.Http.Json;
using System.Web;

public class AuthClientService(HttpClient http)
{
    public async Task<Result<ResultSuccess>> Register(RegisterRequest request)
    {
        return await http.PostAsJsonAsync("/security/register", request).WrapResponse();
    }

    public async Task<Result<AccessTokenResponse?>> Login(LoginRequest request, bool? useCookies = null)
    {
        var q = HttpUtility.ParseQueryString(string.Empty);
        q.Add("useCookies", useCookies?.ToString());
        return await http.PostAsJsonAsync("/security/login?" + q.ToString(), request).WrapResponse<AccessTokenResponse?>();
    }

    public async Task<Result<ResultSuccess>> Logout(string? returnUrl)
    {
        return await http.PostAsJsonAsync("/security/logout", new { returnUrl }).WrapResponse();
    }

    public async Task<Result<AccessTokenResponse>> Refresh(RefreshRequest request)
    {
        return await http.PostAsJsonAsync("/security/refresh", request).WrapResponse<AccessTokenResponse>();
    }

    public async Task<Result<ResultSuccess>> ConfirmEmail(string userId, string code, string? changedEmail)
    {
        var q = HttpUtility.ParseQueryString(string.Empty);
        q.Add("userId", userId);
        q.Add("code", code);
        q.Add("changedEmail", changedEmail);
        return await http.PostAsync("/security/confirmEmail?" + q, null).WrapResponse();
    }

    public async Task<Result<ResultSuccess>> ForgotPassword(ForgotPasswordRequest request)
    {
        return await http.PostAsJsonAsync("/security/forgotPassword", request).WrapResponse();
    }

    public async Task<Result<ResultSuccess>> ResetPassword(ResetPasswordRequest request)
    {
        return await http.PostAsJsonAsync("/security/resetPassword", request).WrapResponse();
    }
}

public record RegisterRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public record AccessTokenResponse
{
    public string TokenType { get; set; }

    public string AccessToken { get; set; }

    public long ExpiresIn { get; set; }

    public string RefreshToken { get; set; }
}

public record LoginRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public string? TwoFactorCode { get; set; }
    public string? TwoFactorRecoveryCode { get; set; }
}

public record RefreshRequest
{
    public required string RefreshToken { get; set; }
}

public record ForgotPasswordRequest
{
    public required string Email { get; set; }
}

public sealed class ResetPasswordRequest
{
    public required string Email { get; set; }
    public required string ResetCode { get; set; }
    public required string NewPassword { get; set; }
}