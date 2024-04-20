using BeanMind.Application.Subject.Queries;
using BeanMind.Application.TodoLists.Queries.GetTodos;
using BeanMind.Application.WeatherForecasts.Queries.GetWeatherForecasts;
using BeanMind.Domain.Entities;
using BeanMind.WebUI.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;
public class SubjectController : ApiControllerBase
{
    [HttpGet]
    public async Task<List<SubjectBriefDTO>> Get()
    {
        return await Mediator.Send(new GetSubjectQuery());
    }
}
