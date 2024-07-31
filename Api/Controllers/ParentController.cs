using Application.Common;
using Application.Parents.Queries;
using Application.Participants.Commands;
using Application.Participants.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route(ControllerRouteName.ParentRoute)]
    public class ParentController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(ISender sender, [FromQuery] GetPaginatedListParentQuery query)
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
            var result = await sender.Send(new GetParentQuery() with { Id = id });
            return new ObjectResult(result)
            {
                StatusCode = result.Code
            };
        }

       
    }
}
