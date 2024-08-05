using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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

    [HttpGet]
    [Route("info/{id}")]
    public async Task<BaseResponse<GetApplicationUserResponseModel>> UserInfoById(ISender sender, [FromRoute] string id)
    {
        var query = new GetApplicationUserQuery
        {
            UserId = id,
        };
        return await sender.Send(query);
    }
    [HttpGet("all-calender/{id}")]
    public async Task<IActionResult> GetCalender(ISender sender, [FromRoute] string id)
    {
        var result = await sender.Send(new GetPaginatedListTeachingSlotByUserIdQuery() with
        {
            Id = id
        });
        return new ObjectResult(result)
        {
            StatusCode = result.Code
        };
    }
    [HttpPost]
    //[Route(RouteNameValues.Register, Name = RouteNameValues.Register)]
    public async Task<BaseResponse<GetBriefApplicationUserResponseModel>> RegisterAccount(ISender sender, [FromBody] RegisterUserCommand command)
    {
        return await sender.Send(command);
    }
    [HttpPut]
    //[Route(RouteNameValues.Register, Name = RouteNameValues.Register)]
    public async Task<BaseResponse<GetBriefApplicationUserResponseModel>> UpdateAccount(ISender sender, [FromBody] UpdateUserCommand command)
    {
        return await sender.Send(command);
    }
}
