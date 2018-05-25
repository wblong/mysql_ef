using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T.MessageQueue.Rabbit
{
    /// <summary>
    /// 消息队列管理类
    /// </summary>
    public static class MessageQueueManager
    {
        private static string IP = "192.168.5.203";
        private static int Port = 5672;
        private static string UserName = "trkj";
        private static string Password = "trkj";

        /// <summary>
        /// 初始化服务器信息
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public static void InitMessageQueueServer(string ip, int port, string userName, string password)
        {
            IP = ip;
            Port = port;
            UserName = userName;
            Password = password;
        }

        /// <summary>
        /// 注册Topic模式的持久化Exchange和Queue
        /// </summary>
        /// <param name="exchangeName">Exchange</param>
        /// <param name="queueName">Queue</param>
        /// <param name="typeName">队列模式</param>
        /// <param name="routeKey">路由Key</param>
        /// <param name="exchangeDurable">Exchange持久化</param>
        /// <param name="exchangeAutoDelete">Exchange断线自动删除</param>
        /// <param name="queueDurable">Queue持久化</param>
        /// <param name="queueAutoDelete">Queue断线自动删除</param>
        /// <param name="queueExclusive">Queue排他性,离线自动删除</param>
        /// <param name="prefetchCount">订阅者预取消息数量</param>
        /// <param name="noAck">无需Ack回复</param>
        /// <returns>消息订阅者</returns>
        public static IQueueSubscriber GerarateIQueueSubscriber(string exchangeName, string queueName, string typeName = "topic", string routeKey = "", bool exchangeDurable = true, bool exchangeAutoDelete = false, bool queueDurable = true, bool queueAutoDelete = false, bool queueExclusive = false, ushort prefetchCount = 0, bool noAck = false, int maxLength = -1, int maxBytes = -1, int messageTtl = -1)
        {
            IQueueSubscriber client = new MessageQueueClient();
            client.Register_durable_Exchange_and_Queue(IP, Port, UserName, Password, exchangeName, queueName, typeName, routeKey, exchangeDurable, exchangeAutoDelete, queueDurable, queueAutoDelete, queueExclusive, prefetchCount, noAck, maxLength, maxBytes, messageTtl);
            return client;
        }

        /// <summary>
        /// 注册Topic模式的持久化Exchange和Queue
        /// </summary>
        /// <param name="exchangeName">Exchange</param>
        /// <param name="queueName">Queue</param>
        /// <param name="typeName">队列模式</param>
        /// <param name="routeKey">路由Key</param>
        /// <param name="exchangeDurable">Exchange持久化</param>
        /// <param name="exchangeAutoDelete">Exchange断线自动删除</param>
        /// <param name="queueDurable">Queue持久化</param>
        /// <param name="queueAutoDelete">Queue断线自动删除</param>
        /// <param name="queueExclusive">Queue排他性</param>
        /// <returns>消息发布者</returns>
        public static IQueuePublisher GerarateIQueuePublisher(string exchangeName, string typeName = "topic", string routeKey = "", bool exchangeDurable = true, bool exchangeAutoDelete = false)
        {
            IQueuePublisher client = new MessageQueueClient();
            client.Register_durable_Exchange_and_Queue(IP, Port, UserName, Password, exchangeName, typeName, routeKey, exchangeDurable, exchangeAutoDelete);
            return client;
        }
    }
}
