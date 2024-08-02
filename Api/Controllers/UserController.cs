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
using Microsoft.AspNetCore.Identity;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Application.QuestionLevels.Queries;
using Domain.Entities.UserEntities;
using Application.Topics.Queries;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Application.Parents;
using Application.ApplicationUsers.Queries;
using Application.ApplicationUsers;

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
}
