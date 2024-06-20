using Application.Common;
using Application.Topics.Commands;
using Application.Topics.Queries;
using Application.Topics;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route(ControllerRouteName.TopicRoute)]
public class TopicController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(ISender sender, [FromQuery] GetPaginatedListTopicQuery query)
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
        var result = await sender.Send(new GetTopicQuery() with
        {
            Id = id
         });
        return new ObjectResult(result)
        {
            StatusCode = result.Code
        };
    }

    [HttpPost]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Create(ISender sender, [FromBody] CreateTopicCommand command)
    {
        var result = await sender.Send(command);
        return new ObjectResult(result)
        {
            StatusCode = result.Code
        };
    }

    [HttpPut]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Update(ISender sender, [FromBody] UpdateTopicCommand command)
    {
        var result = await sender.Send(command);
        return new ObjectResult(result)
        {
            StatusCode = result.Code
        };
    }

    [HttpDelete("{id}")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Delete(ISender sender, [FromRoute] Guid id)
    {
        var result = await sender.Send(new DeleteTopicCommand() with
        {
            Id = id
         });
        return new ObjectResult(result)
        {
            StatusCode = result.Code
        };
    }
}

