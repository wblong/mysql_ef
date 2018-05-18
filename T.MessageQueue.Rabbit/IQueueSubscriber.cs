using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T.MessageQueue.Rabbit
{
    /// <summary>
    /// RabbitMQ消息订阅者接口定义
    /// </summary>
    public interface IQueueSubscriber : IDisposable
    {
        /// <summary>
        /// 消息回调
        /// </summary>
        event Action<IQueueSubscriber, ulong, byte[]> MessageCallback;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverIp"></param>
        /// <param name="serverPort"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="exchangeName"></param>
        /// <param name="queueName"></param>
        /// <param name="typeName"></param>
        /// <param name="routeKey"></param>
        /// <param name="exchangeDurable"></param>
        /// <param name="exchangeAutoDelete"></param>
        /// <param name="queueDurable"></param>
        /// <param name="queueAutoDelete"></param>
        /// <param name="queueExclusive"></param>
        /// <param name="prefetchCount"></param>
        /// <param name="noAck"></param>
        /// <param name="maxLength"></param>
        /// <param name="maxBytes"></param>
        /// <param name="messageTtl"></param>
        /// <returns></returns>
        bool Register_durable_Exchange_and_Queue(string serverIp, int serverPort, string userName, string password, string exchangeName, string queueName, string typeName = "topic", string routeKey = "", bool exchangeDurable = true, bool exchangeAutoDelete = false, bool queueDurable = true, bool queueAutoDelete = false, bool queueExclusive = false, ushort prefetchCount = 0, bool noAck = false, int maxLength = -1, int maxBytes = -1, int messageTtl = -1);

        /// <summary>
        /// 消息ACK回复
        /// </summary>
        /// <param name="deliveryTag"></param>
        /// <param name="multiple"></param>
        /// <returns></returns>
        bool AckAnswer(ulong deliveryTag, bool multiple = false);

    }
}
