using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace WT.Trigger.Application.Common.Interfaces
{
    public interface IMQProducer
    {

        /// <summary>
        /// 处理简单格式消息
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="topic"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ProduceAsync(string topic, string value, CancellationToken cancellationToken);
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
        void Produce(string topic, string value);
        /// <summary>
        /// Kafka发布消息 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value">消息数据体</param>
        void Produce<TValue>(TValue value) where TValue : IRequest;
    }
}
