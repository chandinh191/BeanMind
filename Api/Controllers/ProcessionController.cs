﻿using Application.Common;
using Application.Participants.Commands;
using Application.Participants.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route(ControllerRouteName.ProcessionRoute)]
    public class ProcessionController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(ISender sender, [FromQuery] GetPaginatedListParticipantQuery query)
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
            var result = await sender.Send(new GetParticipantQuery() with { Id = id });
            return new ObjectResult(result)
            {
                StatusCode = result.Code
            };
        }

        [HttpPost]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Create(ISender sender, [FromBody] CreateParticipantCommand command)
        {
            var result = await sender.Send(command);
            return new ObjectResult(result)
            {
                StatusCode = result.Code
            };
        }

        [HttpPut]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Update(ISender sender, [FromBody] UpdateParticipantCommand command)
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
            var result = await sender.Send(new DeleteParticipantCommand() with
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