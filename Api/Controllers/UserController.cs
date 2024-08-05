using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Common.VNPAY_CS_ASPX;
using Application.Users.Commands;
using Application.Users.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Application.Common;
using Application.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Application.QuestionLevels.Queries;
using Domain.Entities.UserEntities;
using Application.Topics.Queries;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Application.Parents;
using Application.ApplicationUsers.Queries;
using Application.ApplicationUsers;
using Application.TeachingSlots.Queries;
using Application.Chapters.Commands;
using Application.ApplicationUsers.Commands;
using Org.BouncyCastle.Asn1.X9;
using Microsoft.IdentityModel.Tokens;
using Application.Topics.Commands;

namespace Api.Controllers;

[ApiController]
[Route(ControllerRouteName.UserRoute)]
public class UserController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<ApplicationUser> _userManager;
    public UserController(IConfiguration configuration, UserManager<ApplicationUser> userManager)
    {
        _configuration = configuration;
        _userManager = userManager;
    }

    /*[HttpGet]
    [Route("{id}")]
    public async Task<BaseResponse<GetApplicationUserResponseModel>> UserInfoById(ISender sender, [FromRoute] string id)
    {
        var query = new GetApplicationUserQuery
        {
            UserId = id,
        };
        return await sender.Send(query);
    }*/
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

    [HttpGet("calender/{id}")]
    public async Task<IActionResult> GetCalender(ISender sender, [FromRoute] string id)
    {
        var result = await sender.Send(new GetPaginatedListSessionByUserIdQuery() with
        {
            Id = id
        });
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
    [HttpPut]
    public async Task<BaseResponse<GetBriefApplicationUserResponseModel>> UpdateAccount(ISender sender, [FromBody] UpdateUserCommand command)
    {

        return await sender.Send(command);
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
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(ISender sender, [FromRoute] string id)
    {
        var result = await sender.Send(new DeleteUserCommand() with
        {
            Id = id
        });
        return new ObjectResult(result)
        {
            StatusCode = result.Code
        };
    }
}
