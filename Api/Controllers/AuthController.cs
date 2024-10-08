﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
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
using Microsoft.AspNetCore.Identity;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Application.QuestionLevels.Queries;
using Domain.Entities.UserEntities;
using Application.Topics.Queries;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Application.Parents;
using Application.ApplicationUsers;
using Application.Auth.Commands;

namespace Api.Controllers;

[ApiController]
[Route(ControllerRouteName.AuthRoute)]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthController(IConfiguration configuration, UserManager<ApplicationUser> userManager)
    {
        _configuration = configuration;
        _userManager = userManager;
    }

    [HttpPost("login-google")]
    public async Task<IActionResult> ConfirmEmailAccount(ISender sender, [FromBody] LoginGoogleCommand command)
    {
        var result = await sender.Send(command);
        return new ObjectResult(result)
        {
            StatusCode = result.Code
        };
    }
    [HttpGet("google")]
    public IActionResult LoginGoogleAPI()
    {
        var props = new AuthenticationProperties { RedirectUri = "/api/v1/auth/google-response" };
        return Challenge(props, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("google-response")]
    public async Task<IActionResult> LoginGoogleResponse(ISender sender)
    {
        var response = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        if (response.Principal == null)
        {
            return BadRequest("Authentication failed");
        }

        var name = response.Principal.FindFirstValue(ClaimTypes.Name);
        var givenName = response.Principal.FindFirstValue(ClaimTypes.GivenName);
        var email = response.Principal.FindFirstValue(ClaimTypes.Email);

        //Do something with the claims
        //Default User
        var defaultUser = new ApplicationUser
        {
            Email = email,
            UserName = email,
            EmailConfirmed = true
        };

        var defaultPassword = _configuration.GetValue<string>("DefaultSettingAccount:Password");
        var defaultUserRole = _configuration.GetValue<string>("DefaultSettingAccount:Role");

        if (_userManager.Users.FirstOrDefault(u => u.Email.Equals(defaultUser.Email)) == null)
        {
            await _userManager.CreateAsync(defaultUser, defaultPassword);
            if (!string.IsNullOrWhiteSpace(defaultUserRole))
            {
                await _userManager.AddToRolesAsync(defaultUser, new[] { defaultUserRole });
            }
        }
        var result = await sender.Send(new LoginCommand() with
        {
            Username = email,
            Password = defaultPassword
        }); 
        return new ObjectResult(result)
        {
            StatusCode = result.Code
        };
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(ISender sender, [FromQuery] GetPaginatedListUserQuery query)
    {
        var result = await sender.Send(query);
        return new ObjectResult(result)
        {
            StatusCode = result.Code
        };
    }

    [HttpGet]
    [Route("info/{id}")]
    public async Task<BaseResponse<GetUserInfoResponseModel>> UserInfoById(ISender sender, [FromRoute] string id)
    {
        var query = new GetUserInfoQuery
        {
            UserId = id,
        };
        return await sender.Send(query);
    }


    [HttpGet("info")]
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

    [HttpGet("confirmEmail")]
    public async Task<IActionResult> ConfirmEmailAccount(ISender sender, [FromQuery] ConfirmEmailCommand command)
    {
        var result = await sender.Send(command);
        if (result.Success) {
            return Redirect("https://firebasestorage.googleapis.com/v0/b/beanmind-2911.appspot.com/o/avatar_images%2Fxacnhanemail.png?alt=media&token=7b1ebe06-4cc1-4c93-b251-5dd4d6ec1f1a");
        }
        return new ObjectResult(result)
        {
            StatusCode = result.Code
        };
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(ISender sender, [FromBody] LoginCommand command)
    {
        var result = await sender.Send(command);
        return new ObjectResult(result)
        {
            StatusCode = result.Code
        };
    }


    [HttpPost("resendConfirmEmail")]
    public async Task<IActionResult> ResendConfirmEmail(ISender sender, [FromBody] ResendConfirmEmailCommand command)
    {
        var result = await sender.Send(command);
        return new ObjectResult(result)
        {
            StatusCode = result.Code
        };
    }

    [HttpPost("sendAccountStudentInfo")]
    public async Task<IActionResult> SendAccountStudentInfo(ISender sender, [FromBody] SendAccountStudentInfoCommand command)
    {
        var result = await sender.Send(command);
        return new ObjectResult(result)
        {
            StatusCode = result.Code
        };
    }

    [HttpPost]
    public async Task<BaseResponse<GetBriefApplicationUserResponseModel>> RegisterAccount(ISender sender, [FromBody] RegisterUserCommand command)
    {
        return await sender.Send(command);
    }

    [HttpPost("refresh")]
    [Authorize]
    public async Task<BaseResponse<AccessTokenResponseModel>> RefreshToken(ISender sender, [FromBody] TokenRefreshCommand command)
    {
        return await sender.Send(command);
    }

    [HttpPost("forgotPassword")]
    [Authorize]
    public async Task<BaseResponse<string>> ForgotPassword(ISender sender, [FromBody] ForgotPasswordCommand command)
    {
        return await sender.Send(command);
    }

    [HttpPost("resetPassword")]
    [Authorize]
    public async Task<BaseResponse<string>> ResetPassword(ISender sender, [FromBody] ResetPasswordCommand command)
    {
        return await sender.Send(command);
    }
}
