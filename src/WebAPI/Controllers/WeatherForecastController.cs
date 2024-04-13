using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WT.DirectLogistics.Application.WeatherForecasts.Queries.GetWeatherForecasts;
using WT.DirectLogistics.WebAPI.Controllers;
using WT.DirectLogistics.WebAPI.Filters;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            return await Mediator.Send(new GetWeatherForecastsQuery());
        }
    }
}
