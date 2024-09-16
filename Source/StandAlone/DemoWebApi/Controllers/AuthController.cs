namespace DemoWebApi.Controllers;

using DemoWebApi.Auth;
using DemoWebApi.Database;
using DemoWebApi.Database.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("[controller]")]
public class AuthController(AuthJwtService authJwtService, AuthRepository authRepository) : ControllerBase
{
    public record LoginDto(string Email, string Password);

    [HttpPost]
    [Route("login")]
    public ActionResult<JwtLoginResponse> Login(bool? useCookies, [FromBody] LoginDto arg)
    {
        var (jwtResponse, principal) = authJwtService.Login(arg.Email, arg.Password);
        if (jwtResponse != null)
        {
            if (useCookies == true)
            {
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal!);
            }
            return jwtResponse;
        }

        return BadRequest("Invalid Login");
    }

    [Authorize]
    [HttpGet]
    [Route("user/info")]
    public ActionResult<UserInfo> GetUserInfo()
    {
        var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = authRepository.GetUserInfo(Convert.ToInt32(id));
        return user!;
    }

    [HttpGet]
    [Route("role")]
    public ActionResult<List<RoleInfo>> GetRoleInfoList()
    {
        return authRepository.GetRoleInfoList();
    }

    [Authorize]
    [HttpPatch]
    [Route("role")]
    public ActionResult<int> ConfigRole([FromBody] ConfigRoleArg arg)
    {
        return authRepository.ConfigRole(arg.Name, arg.DisplayName);
    }

    [Authorize]
    [HttpPatch]
    [Route("role/{roleId}/permissions")]
    public ActionResult<int> ConfigRolePermissions(int roleId, [FromBody] ConfigRolePermissionsArg arg)
    {
        return authRepository.ConfigRolePermissions(roleId, arg.Permissions);
    }

    [Authorize]
    [HttpPatch]
    [Route("user/{userId}/role")]
    public ActionResult<int> ConfigUserRoles(int userId, [FromBody] ConfigUserRolesArg arg)
    {
        return authRepository.ConfigUserRoles(userId, arg.RoleIds);
    }
}

public record ConfigRoleArg(string Name, string DisplayName);
public record ConfigRolePermissionsArg(string[] Permissions);
public record ConfigUserRolesArg(int[] RoleIds);
