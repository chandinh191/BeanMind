using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Application.Users.Commands;
using Application.Users.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Application.Common;
using Application.Users;
using MediatR;
using Application.Courses.Queries;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Api.Controllers;

[ApiController]
[Route(ControllerRouteName.UserRoute)]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    [HttpGet("login")]
    public IActionResult Login()
    {
        var props = new AuthenticationProperties { RedirectUri = "/api/v1/auth/login-google" };
        return Challenge(props, GoogleDefaults.AuthenticationScheme);
    }
    [HttpGet("login-google")]
    public async Task<IActionResult> GoogleLogin()
    {
        var response = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        if (response.Principal == null) return BadRequest();

        var name = response.Principal.FindFirstValue(ClaimTypes.Name);
        var givenName = response.Principal.FindFirstValue(ClaimTypes.GivenName);
        var email = response.Principal.FindFirstValue(ClaimTypes.Email);
        //Do something with the claims
        // var user = await UserService.FindOrCreate(new { name, givenName, email});

        return Ok(response);
    }

    [HttpGet]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("get-all")]
    public async Task<IActionResult> GetAll(ISender sender, [FromQuery] GetPaginatedListUserQuery query)
    {
        var result = await sender.Send(query);
        return new ObjectResult(result)
        {
            StatusCode = result.Code
        };
    }

    [HttpGet]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route(RouteNameValues.Info, Name = RouteNameValues.Info)]
    public async Task<BaseResponse<GetUserInfoResponseModel>> UserInfo(ISender sender)
    {
        // get claim from user object
        var claimIdentity = HttpContext.User.Identity as ClaimsIdentity;

        var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var query = new GetUserInfoQuery
        {
            UserId = userId,
        };

        return await sender.Send(query);
    }

    [HttpGet]
    [Route(RouteNameValues.ConfirmEmail, Name = RouteNameValues.ConfirmEmail)]
    public async Task<BaseResponse<string>> ConfirmEmailAccount(ISender sender, [FromQuery] ConfirmEmailCommand command)
    {
        return await sender.Send(command);
    }

    [HttpPost]
    [Route(RouteNameValues.Register, Name = RouteNameValues.Register)]
    public async Task<BaseResponse<string>> RegisterAccount(ISender sender, [FromBody] RegisterUserCommand command)
    {
        return await sender.Send(command);
    }

    [HttpPost]
    [Route(RouteNameValues.Login, Name = RouteNameValues.Login)]
    public async Task<BaseResponse<AccessTokenResponseModel>> Login(ISender sender, [FromBody] LoginCommand command)
    {
        return await sender.Send(command);
    }

    [HttpPost]
    [Route(RouteNameValues.LoginGoogle, Name = RouteNameValues.LoginGoogle)]
    public async Task<BaseResponse<AccessTokenResponseModel>> LoginGoogle(ISender sender, [FromBody] LoginGoogleCommand command)
    {
        return await sender.Send(command);
    }

    [HttpPost]
    [Route(RouteNameValues.ResendConfirmEmail, Name = RouteNameValues.ResendConfirmEmail)]
    public async Task<BaseResponse<string>> ResendConfirmEmail(ISender sender, [FromBody] ResendConfirmEmailCommand command)
    {
        return await sender.Send(command);
    }

    [HttpPost]
    [Route(RouteNameValues.Refresh, Name = RouteNameValues.Refresh)]
    public async Task<BaseResponse<AccessTokenResponseModel>> RefreshToken(ISender sender, [FromBody] TokenRefreshCommand command)
    {
        return await sender.Send(command);
    }

    [HttpPost]
    [Route(RouteNameValues.ForgotPassword, Name = RouteNameValues.ForgotPassword)]
    public async Task<BaseResponse<string>> ForgotPassword(ISender sender, [FromBody] ForgotPasswordCommand command)
    {
        return await sender.Send(command);
    }

    [HttpPost]
    [Route(RouteNameValues.ResetPassword, Name = RouteNameValues.ResetPassword)]
    public async Task<BaseResponse<string>> ResetPassword(ISender sender, [FromBody] ResetPasswordCommand command)
    {
        return await sender.Send(command);
    }
}
