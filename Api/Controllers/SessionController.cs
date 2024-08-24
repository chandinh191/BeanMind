using Application.Common;
using Application.Sessions.Commands;
using Application.Sessions.Queries;
using Application.Subjects.Commands;
using Application.Subjects.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route(ControllerRouteName.SessionRoute)]
    public class SessionController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(ISender sender, [FromQuery] GetPaginatedListSessionQuery query)
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
            var result = await sender.Send(new GetSessionQuery() with
            {
                Id = id
            });
            return new ObjectResult(result)
            {
                StatusCode = result.Code
            };
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(ISender sender, [FromBody] CreateSessionCommand command)
        {
            var result = await sender.Send(command);
            return new ObjectResult(result)
            {
                StatusCode = result.Code
            };
        }
        [HttpPost("auto-assign")]
        //[Authorize]
        public async Task<IActionResult> CreateAutoAssign(ISender sender, [FromBody] CreateAutoSessionCommand command)
        {
            var result = await sender.Send(command);
            return new ObjectResult(result)
            {
                StatusCode = result.Code
            };
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update(ISender sender, [FromBody] UpdateSessionCommand command)
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
            var result = await sender.Send(new DeleteSessionCommand() with
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
