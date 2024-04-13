using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Logging;
using NPOI.OpenXmlFormats.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WT.Trigger.Application.Common.Interfaces;

namespace WT.Trigger.Infrastructure.Kafka
{
    public class KafkaProducer : IMQProducer
    {
        private readonly IProducer<Null, byte[]> _kafkaHandle;
        private readonly ILogger<KafkaProducer> _logger;

        public KafkaProducer(KafkaClientHandle handle, ILogger<KafkaProducer> logger)
        {
            _kafkaHandle = new DependentProducerBuilder<Null, byte[]>(handle.Handle).Build();
            _logger = logger;
        }

        public Task ProduceAsync<TValue>(TValue value, CancellationToken cancellationToken) where TValue : IRequest
        {
            return ProduceAsync(typeof(TValue).Name, value, cancellationToken);
        }

        public Task ProduceAsync<TValue>(string topic, TValue value, CancellationToken cancellationToken) where TValue : IRequest
        {
            var message = new Message<Null, byte[]>
            {
                Value = JsonSerializer.SerializeToUtf8Bytes(value)
            };

            _logger.LogInformation("Publishing Message. Topic - {topic}", topic);

            return _kafkaHandle.ProduceAsync(topic, message, cancellationToken);
        }

        public void Produce<TValue>(string topic, TValue value) where TValue : IRequest
        {
            var message = new Message<Null, byte[]>
            {
                Value = JsonSerializer.SerializeToUtf8Bytes(value)
            };

            _logger.LogInformation("Publishing Message. Topic - {topic}", topic);

            _kafkaHandle.Produce(topic, message);
        }

        public void Produce<TValue>(TValue value) where TValue : IRequest
        {
            Produce(typeof(TValue).Name, value);
        }

        public void Flush(TimeSpan timeout)
            => _kafkaHandle.Flush(timeout);

        public Task ProduceAsync(string topic, string value, CancellationToken cancellationToken)
        {
            var message = new Message<Null, byte[]>
            {
                Value = JsonSerializer.SerializeToUtf8Bytes(value)
            };

            _logger.LogInformation("Publishing Message. Topic - {topic}", topic);

            return _kafkaHandle.ProduceAsync(topic, message, cancellationToken);

        }

        public void Produce(string topic, string value)
        {
            byte[] bytes= System.Text.Encoding.UTF8.GetBytes(value);
            _kafkaHandle.Produce(topic, new Message<Null, byte[]> { Value = bytes });

        }
    }

}
