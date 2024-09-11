namespace DemoWebApi.Controllers;

using DemoWebApi.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class AuthController(AuthJwtService authJwtService) : ControllerBase
{
    public record LoginDto(string Email, string Password);

    [HttpPost]
    [Route("login")]
    public ActionResult<JwtLoginResponse> Login([FromBody] LoginDto arg)
    {
        var res = authJwtService.Login(arg.Email, arg.Password);
        if (res != null)
        {
            return Ok(res);
        }

        return BadRequest("Invalid Login");
    }

    [Authorize]
    [HttpGet]
    [Route("userInfo")]
    public ActionResult GetUserInfo()
    {
        return Ok(User.Claims.Select(c => c.ToString()).ToList());
    }
}
