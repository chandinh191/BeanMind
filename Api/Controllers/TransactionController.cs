using Application.Common;
using Application.Topics.Commands;
using Application.Topics.Queries;
using Application.Transactions.Commands;
using Application.Transactions.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route(ControllerRouteName.TransactionRoute)]
    public class TransactionController : ControllerBase
    {
        [HttpGet]
        //[Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetAll(ISender sender, [FromQuery] GetPaginatedListTransactionQuery query)
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
        public async Task<IActionResult> Create(ISender sender, [FromBody] CreateTransactionCommand command)
        {
            var result = await sender.Send(command);
            return new ObjectResult(result)
            {
                StatusCode = result.Code
            };
        }

        [HttpPut]
        public async Task<IActionResult> Update(ISender sender, [FromBody] UpdateTransactionCommand command)
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
            var result = await sender.Send(new DeleteTransactionCommand() with
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
