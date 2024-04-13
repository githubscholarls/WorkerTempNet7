using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Hosting;
using WT.Trigger.Application.Common.Interfaces;
using System;
using WT.Trigger.Application.WeatherForecasts.Command;

namespace WT.Trigger.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly IMQCustomer _customer;
        private readonly IMQProducer _producer;

        public Worker(IMQCustomer customer, IMQProducer producer)
        {
            _customer = customer;
            _producer = producer;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("当前执行时间" + DateTime.Now);
            //创建当前topic
            _customer.CreateTopics("wt.test");
            //投递
            _producer.ProduceAsync<SetWeatherForecastsCommand>("wt.test", new SetWeatherForecastsCommand() { days = 1 }, stoppingToken);
            //消费
            _customer.Subscribe<SetWeatherForecastsCommand>("wt.test", stoppingToken);
            return Task.CompletedTask;
        }
    }
}
