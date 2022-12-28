using MicroserviceTemplate.Application.WeatherForecasts.Queries.GetWeatherForecasts;
using MicroserviceTemplate.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace MicroserviceTemplate.WebUI.Controllers;

public class WeatherForecastController : ApiControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        return await Mediator.Send(new GetWeatherForecastsQuery());
    }
}
