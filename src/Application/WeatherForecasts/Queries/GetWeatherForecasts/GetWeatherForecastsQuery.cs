using CSRedis;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WT.DirectLogistics.Application.Common.Interfaces;

namespace WT.DirectLogistics.Application.WeatherForecasts.Queries.GetWeatherForecasts
{
    public class GetWeatherForecastsQuery : IRequest<IEnumerable<WeatherForecast>>
    {
    }

    public class GetWeatherForecastsQueryHandler : IRequestHandler<GetWeatherForecastsQuery, IEnumerable<WeatherForecast>>
    {
        private readonly CSRedisClient _redisClient;
        private readonly ILoginHelper _iLoginHelper; 

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public GetWeatherForecastsQueryHandler(CSRedisClient redisClient, ILoginHelper iLoginHelper)
        {
            _redisClient = redisClient;
            _iLoginHelper = iLoginHelper;
        }

        public async Task<IEnumerable<WeatherForecast>> Handle(GetWeatherForecastsQuery request, CancellationToken cancellationToken)
        {
           var huiyuan=  await _iLoginHelper.IsLogin();
            if (_redisClient.Exists("testkey"))
            {
               var data= _redisClient.Get<IEnumerable<WeatherForecast>>("testkey");

                return await Task.FromResult(data);
            }

            var rng = new Random();

            var vm = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });

            _redisClient.Set("testkey", vm, TimeSpan.FromSeconds(3));

            return await Task.FromResult(vm);
        }
    }
}
