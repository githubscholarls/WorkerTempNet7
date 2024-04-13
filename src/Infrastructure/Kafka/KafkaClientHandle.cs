using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace WT.Trigger.Infrastructure.Kafka
{
    public class KafkaClientHandle : IDisposable
    {
        private readonly IProducer<byte[], byte[]> _producer;

        public KafkaClientHandle(IOptions<ProducerConfig> producerConfig)
        {
            _producer = new ProducerBuilder<byte[], byte[]>(producerConfig.Value).Build();
        }

        public Handle Handle => _producer.Handle;

        public void Dispose()
        {
            _producer.Flush();
            _producer.Dispose();
        }
    }

}
