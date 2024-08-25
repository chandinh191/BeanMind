using Application.Common;
using Application.Worksheets.Commands;
using Application.Worksheets.Queries;
using Application.Worksheets;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route(ControllerRouteName.WorksheetRoute)]
public class WorksheetController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(ISender sender, [FromQuery] GetPaginatedListWorksheetQuery query)
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
        var result = await sender.Send(new GetWorksheetQuery() with
        {
            Id = id
         });
        return new ObjectResult(result)
        {
            StatusCode = result.Code
        };
    }

    [HttpPost]
    //[Authorize]
    public async Task<IActionResult> Create(ISender sender, [FromBody] CreateWorksheetCommand command)
    {
        var result = await sender.Send(command);
        return new ObjectResult(result)
        {
            StatusCode = result.Code
        };
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Update(ISender sender, [FromBody] UpdateWorksheetCommand command)
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
        var result = await sender.Send(new DeleteWorksheetCommand() with
        {
            Id = id
         });
        return new ObjectResult(result)
        {
            StatusCode = result.Code
        };
    }
}


