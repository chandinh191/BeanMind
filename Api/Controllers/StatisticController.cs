using Application.Common;
using Application.Parents.Queries;
using Application.Sessions.Commands;
using Application.Sessions.Queries;
using Application.Statistic.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route(ControllerRouteName.StatisticRoute)]
    public class StatisticController : ControllerBase
    {
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetAll(ISender sender)
        {
            var result = await sender.Send(new GetDashboardQuery());
            return new ObjectResult(result)
            {
                StatusCode = result.Code
            };
        }

    }
}
