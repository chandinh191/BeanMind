using Application.Common;
using Application.QuestionLevels.Commands;
using Application.QuestionLevels.Queries;
using Application.QuestionLevels;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route(ControllerRouteName.QuestionLevelRoute)]
public class QuestionLevelController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(ISender sender, [FromQuery] GetPaginatedListQuestionLevelQuery query)
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
        var result = await sender.Send(new GetQuestionLevelQuery() with
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
    public async Task<IActionResult> Create(ISender sender, [FromBody] CreateQuestionLevelCommand command)
    {
        var result = await sender.Send(command);
        return new ObjectResult(result)
        {
            StatusCode = result.Code
        };
    }

    [HttpPut]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Update(ISender sender, [FromBody] UpdateQuestionLevelCommand command)
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
        var result = await sender.Send(new DeleteQuestionLevelCommand() with
        {
            Id = id
         });
        return new ObjectResult(result)
        {
            StatusCode = result.Code
        };
    }
}
