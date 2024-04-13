using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WT.DirectLogistics.Application.Common.Interfaces
{
    public interface IMQProducer
    {
        /// <summary>
        /// Kafka异步发布消息 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="topic">Kafka主题</param>
        /// <param name="value">消息数据体</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ProduceAsync<TValue>(string topic, TValue value, CancellationToken cancellationToken) where TValue : IRequest;

        /// <summary>
        /// Kafka异步发布消息 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="topic">Kafka主题</param>
        /// <param name="value">消息数据体</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ProduceAsync<TValue>(TValue value, CancellationToken cancellationToken) where TValue : IRequest;

        /// <summary>
        /// Kafka发布消息 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="topic">Kafka主题</param>
        /// <param name="value">消息数据体</param>
        void Produce<TValue>(string topic, TValue value) where TValue : IRequest;

        /// <summary>
        /// Kafka发布消息 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value">消息数据体</param>
        void Produce<TValue>(TValue value) where TValue : IRequest;
    }
}
