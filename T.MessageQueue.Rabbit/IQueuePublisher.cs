using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T.MessageQueue.Rabbit
{
    /// <summary>
    /// RabbitMQ消息发送者接口定义
    /// </summary>
    public interface IQueuePublisher : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverIp"></param>
        /// <param name="serverPort"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="exchangeName"></param>
        /// <param name="typeName"></param>
        /// <param name="routeKey"></param>
        /// <param name="exchangeDurable"></param>
        /// <param name="exchangeAutoDelete"></param>
        /// <returns></returns>
        bool Register_durable_Exchange_and_Queue(string serverIp, int serverPort, string userName, string password, string exchangeName, string typeName = "topic", string routeKey = "", bool exchangeDurable = true, bool exchangeAutoDelete = false);

        /// <summary>
        /// 异步消息入队
        /// </summary>
        /// <param name="message">消息体</param>
        /// <param name="persistent">持久化</param>
        /// <returns></returns>
        Task<bool> MessageEnqueueAsync(string message, bool persistent = true);

        /// <summary>
        /// 消息入队
        /// </summary>
        /// <param name="message">消息体</param>
        /// <param name="persistent">持久化</param>
        /// <returns></returns>
        bool MessageEnqueue(string message, bool persistent = true);

    }
}
