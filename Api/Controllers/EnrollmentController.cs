﻿using Application.Chapters.Commands;
using Application.Chapters.Queries;
using Application.Common;
using Application.Enrollments.Commands;
using Application.Enrollments.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route(ControllerRouteName.EnrollmentRoute)]
    public class EnrollmentController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(ISender sender, [FromQuery] GetPaginatedListEnrollmentQuery query)
        {
            var result = await sender.Send(query);
            return new ObjectResult(result)
            {
                StatusCode = result.Code
            };
        }
        [HttpGet("simplify")]
        public async Task<IActionResult> GetAll2(ISender sender, [FromQuery] GetPaginatedListEnrollmentQueryV2 query)
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
            var result = await sender.Send(new GetEnrollmentQuery() with { Id = id });
            return new ObjectResult(result)
            {
                StatusCode = result.Code
            };
        }

        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> Create(ISender sender, [FromBody] CreateEnrollmentCommand command)
        {
            var result = await sender.Send(command);
            return new ObjectResult(result)
            {
                StatusCode = result.Code
            };
        }


        [HttpPost("enrollment-able")]
        public async Task<IActionResult> IsEnroll(ISender sender, [FromBody] CheckAbleEnrollmentCommand command)
        {
            var result = await sender.Send(command);
            return new ObjectResult(result)
            {
                StatusCode = result.Code
            };
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update(ISender sender, [FromBody] UpdateEnrollmentCommand command)
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
            var result = await sender.Send(new DeleteEnrollmentCommand() with
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
