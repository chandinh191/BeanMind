﻿using Application.Common;
using Application.Enrollments.Commands;
using Application.Enrollments.Queries;
using Application.Games.Commands;
using Application.Games.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route(ControllerRouteName.GameRoute)]
    public class GameController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(ISender sender, [FromQuery] GetPaginatedListGameQuery query)
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
            var result = await sender.Send(new GetGameQuery() with { Id = id });
            return new ObjectResult(result)
            {
                StatusCode = result.Code
            };
        }

        [HttpPost]
        public async Task<IActionResult> Create(ISender sender, [FromBody] CreateGameCommand command)
        {
            var result = await sender.Send(command);
            return new ObjectResult(result)
            {
                StatusCode = result.Code
            };
        }

        [HttpPut]
        public async Task<IActionResult> Update(ISender sender, [FromBody] UpdateGameCommand command)
        {
            var result = await sender.Send(command);
            return new ObjectResult(result)
            {
                StatusCode = result.Code
            };
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(ISender sender, [FromRoute] Guid id)
        {
            var result = await sender.Send(new DeleteGameCommand() with
            {
                Id = id
            });
            return new ObjectResult(result)
            {
                StatusCode = result.Code
            };
        }
    }
}
