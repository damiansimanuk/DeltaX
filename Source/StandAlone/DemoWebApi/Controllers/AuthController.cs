namespace DemoWebApi.Controllers;

using DemoWebApi.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class AuthController(AuthJwtService authJwtService) : ControllerBase
{
    public record LoginDto(string Email, string Password);

    [HttpPost]
    [Route("login")]
    public ActionResult<JwtLoginResponse> Login(bool? useCookies, [FromBody] LoginDto arg)
    {
        var res = authJwtService.Login(arg.Email, arg.Password, out var principal);
        if (res != null)
        {
            if (useCookies == true)
            {
                Console.WriteLine(" Signing in...");
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal!);
            }
            return res;
        }

        return BadRequest("Invalid Login");
    }

    [Authorize]
    [HttpGet]
    [Route("userInfo")]
    public ActionResult<Dictionary<string, string>> GetUserInfo()
    {
        return User.Claims.ToDictionary(c => c.Type, c => c.Value);
    }
}
