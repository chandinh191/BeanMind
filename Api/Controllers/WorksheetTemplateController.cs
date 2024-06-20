using Application.WorksheetTemplates.Commands;
using Application.WorksheetTemplates.Queries;
using Application.WorksheetTemplates;
using Application.Common;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route(ControllerRouteName.WorksheetTemplateRoute)]
public class WorksheetTemplateController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(ISender sender, [FromQuery] GetPaginatedListWorksheetTemplateQuery query)
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
        var result = await sender.Send(new GetWorksheetTemplateQuery() with
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
    public async Task<IActionResult> Create(ISender sender, [FromBody] CreateWorksheetTemplateCommand command)
    {
        var result = await sender.Send(command);
        return new ObjectResult(result)
        {
            StatusCode = result.Code
        };
    }

    [HttpPut]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Update(ISender sender, [FromBody] UpdateWorksheetTemplateCommand command)
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
        var result = await sender.Send(new DeleteWorksheetTemplateCommand() with
        {
            Id = id
         });
        return new ObjectResult(result)
        {
            StatusCode = result.Code
        };
    }
}
