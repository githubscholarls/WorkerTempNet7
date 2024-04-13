using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WT.Trigger.Application.Common.Common;
using WT.Trigger.Application.Common.Interfaces;

namespace WT.Trigger.Infrastructure.Kafka
{
    public class KafkaCustomer : IMQCustomer
    {
        private readonly ILogger<KafkaCustomer> _logger;
        private readonly IServiceProvider ServiceProvider;
        private readonly Dictionary<string, Type> topicPairs = new();
        private readonly IOptions<ConsumerConfig> _consumerConfig;

        public KafkaCustomer(IOptions<ConsumerConfig> options, ILogger<KafkaCustomer> logger, IServiceProvider services)
        {
            _consumerConfig = options;
            _logger = logger;
            ServiceProvider = services;
        }


        public void Subscribe<TValue>(string topic, CancellationToken cancellationToken) where TValue : IRequest
        {
            Subscribe<TValue>(topic, _consumerConfig.Value.GroupId, cancellationToken);
        }


        public void Subscribe<TValue>(string topic, string groupId, CancellationToken cancellationToken) where TValue : IRequest
        {
            topicPairs.Add(topic, typeof(TValue));

            Consume(topic, groupId, cancellationToken);
        }
        /// <summary>
        /// 线上创建topic
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        public async Task CreateTopics(string topic)
        {
            AdminClientConfig adminClientConfig = new()
            {
                BootstrapServers = _consumerConfig.Value.BootstrapServers,
            };

            var bu = new AdminClientBuilder(adminClientConfig).Build();
            await bu.CreateTopicsAsync(new TopicSpecification[] {
                                         new TopicSpecification { Name = topic}
                                       });
        }
        private void Consume(string topic, string groupId, CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _consumerConfig.Value.BootstrapServers,
                GroupId = groupId ?? _consumerConfig.Value.GroupId,
                EnableAutoCommit = true,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            var kafkaConsumer = new ConsumerBuilder<string, byte[]>(config).Build();
            kafkaConsumer.Subscribe(topic);

            Task.Factory.StartNew(async () =>
            {
                _logger.LogInformation("Kafka Has Subscribe Topic: {topic} GroupId：{GroupId}", topic, config.GroupId);

                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var cr = kafkaConsumer.Consume(cancellationToken);
                        string v = System.Text.Encoding.UTF8.GetString(cr.Message.Value);
                        if (JsonHelper.IsJson(v, topicPairs[cr.Topic]))
                        {
                            var value = JsonSerializer.Deserialize(cr.Message.Value, topicPairs[cr.Topic]);
                            using var scope = ServiceProvider.CreateScope();
                            var mediator =
                                scope.ServiceProvider
                                    .GetRequiredService<ISender>();
                            await mediator.Send(value, cancellationToken);
                        }
                        else {
                            var value = ConvertToObjectHelper.GetModel(v, topicPairs[cr.Topic]);
                            using var scope = ServiceProvider.CreateScope();
                            var mediator =
                                scope.ServiceProvider
                                    .GetRequiredService<ISender>();
                            await mediator.Send(value, cancellationToken);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                    catch (ConsumeException e)
                    {
                        _logger.LogError(e, "Kafka Consume error");

                        if (e.Error.IsFatal)
                        {
                            break;
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }
    }

}
