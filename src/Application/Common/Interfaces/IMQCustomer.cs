using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace WT.Trigger.Application.Common.Interfaces
{
    public interface IMQCustomer
    {
        /// <summary>
        /// Kafka消费
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="topic">Kafka主题</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        void Subscribe<TValue>(string topic, CancellationToken cancellationToken) where TValue : IRequest;

        /// <summary>
        /// Kafka消费
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="topic">Kafka topic</param>
        /// <param name="groupId">Kafka groupId</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        void Subscribe<TValue>(string topic, string groupId, CancellationToken cancellationToken)
            where TValue : IRequest;

        Task CreateTopics(string topic);
    }

}
