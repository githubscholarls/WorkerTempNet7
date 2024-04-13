using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WT.Trigger.Application.WeatherForecasts.Command
{
    public class SetWeatherForecastsCommand:IRequest
    {
        public int days { get; set; }
    }

    public class SetWeatherForecastsCommandHandler : IRequestHandler<SetWeatherForecastsCommand>
    {
        public async Task<Unit> Handle(SetWeatherForecastsCommand request, CancellationToken cancellationToken)
        {
            Console.WriteLine("当前days"+request.days);
            return await Task.FromResult(Unit.Value);
        }
    }
}
