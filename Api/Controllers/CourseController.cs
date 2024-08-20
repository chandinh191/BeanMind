using Application.Common;
using Application.Courses.Commands;
using Application.Courses.Queries;
using Application.Courses;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]

[Route(ControllerRouteName.CourseRoute)]
public class CourseController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(ISender sender, [FromQuery] GetPaginatedListCourseQuery query)
    {
        var result = await sender.Send(query);
        return new ObjectResult(result)
        {
            StatusCode = result.Code
        };
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(ISender sender, [FromRoute] Guid id)
    {
        var result = await sender.Send(new GetCourseQuery() with { Id = id });
        return new ObjectResult(result)
        {
            StatusCode = result.Code
        };
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(ISender sender, [FromBody] CreateCourseCommand command)
    {
        var result = await sender.Send(command);
        return new ObjectResult(result)
        {
            StatusCode = result.Code
        };
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Update(ISender sender, [FromBody] UpdateCourseCommand command)
    {
        var result = await sender.Send(command);
        return new ObjectResult(result)
        {
            StatusCode = result.Code
        };
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(ISender sender, [FromRoute] Guid id)
    {
        var result = await sender.Send(new DeleteCourseCommand() with
        {
            Id = id
        });
        return new ObjectResult(result)
        {
            StatusCode = result.Code
        };
    }
}

