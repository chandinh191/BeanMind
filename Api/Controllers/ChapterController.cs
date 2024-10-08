﻿using Application.Common;
using Application.Chapters.Commands;
using Application.Chapters.Queries;
using Application.Chapters;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route(ControllerRouteName.ChapterRoute)]
public class ChapterController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(ISender sender, [FromQuery] GetPaginatedListChapterQuery query)
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
        var result = await sender.Send(new GetChapterQuery() with { Id = id });
        return new ObjectResult(result)
        {
            StatusCode = result.Code
        };
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(ISender sender, [FromBody] CreateChapterCommand command)
    {
        var result = await sender.Send(command);
        return new ObjectResult(result)
        {
            StatusCode = result.Code
        };
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Update(ISender sender, [FromBody] UpdateChapterCommand command)
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
        var result = await sender.Send(new DeleteChapterCommand() with
        {
            Id = id
        });
        return new ObjectResult(result)
        {
            StatusCode = result.Code
        };
    }
}

