using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace T.MessageQueue.Rabbit
{
    /// <summary>
    /// RabbitMQ发送订阅接口实现类
    /// </summary>
    public class MessageQueueClient : IQueueSubscriber, IQueuePublisher
    {
        /// <summary>
        /// 服务地址
        /// </summary>
        private string ServerIp;
        /// <summary>
        /// 服务端口
        /// </summary>
        private int ServerPort;
        /// <summary>
        /// 用户名
        /// </summary>
        private string UserName;
        /// <summary>
        /// 密码
        /// </summary>
        private string Password;
        /// <summary>
        /// Exchange的名称
        /// </summary>
        private string ExchangeName;
        /// <summary>
        /// Queue的名称
        /// </summary>
        private string QueueName;
        /// <summary>
        /// 类型
        /// </summary>
        private string TypeName;
        /// <summary>
        /// 路由Key名称
        /// </summary>
        private string RoutingKey;
        /// <summary>
        /// Exchange持久化
        /// </summary>
        private bool ExchangeDurable = true;
        /// <summary>
        /// Exchange自动删除
        /// </summary>
        private bool ExchangeAutoDelete = false;
        /// <summary>
        /// Queue持久化
        /// </summary>
        private bool QueueDurable = true;
        /// <summary>
        /// Queue自动删除
        /// </summary>
        private bool QueueAutoDelete = false;
        /// <summary>
        /// Queue排他性
        /// </summary>
        private bool QueueExclusive = false;
        /// <summary>
        /// 无需Ack回复
        /// </summary>
        private bool NoAck = false;
        /// <summary>
        /// 订阅者预取总数
        /// </summary>
        private ushort PrefetchCount = 0;
        /// <summary>
        /// 队列最大记录条数
        /// </summary>
        private int QueueMaxLength = -1;
        /// <summary>
        /// 队列最大消息容量
        /// </summary>
        private int QueueMaxBytes = -1;
        /// <summary>
        /// 队列消息存活时间
        /// </summary>
        private int QueueMessageTtl = -1;
        /// <summary>
        /// 订阅者模式
        /// </summary>
        private bool IsSubscriber = false;
        /// <summary>
        /// 
        /// </summary>
        private IBasicProperties Props;

        /// <summary>
        /// 通道
        /// </summary>
        private IModel channel;
        /// <summary>
        /// 连接
        /// </summary>
        private IConnection connection;
        /// <summary>
        /// 基于事件订阅者
        /// </summary>
        private EventingBasicConsumer eventintBasicConsumer;
        /// <summary>
        /// 消息回调事件委托
        /// </summary>
        public event Action<IQueueSubscriber, ulong, byte[]> MessageCallback;

        /// <summary>
        /// 当前缓存的连接信息
        /// </summary>
        public static List<IConnectionResource> IConnectionResourceList = new List<IConnectionResource>();
        /// <summary>
        /// 线程安全锁对象
        /// </summary>
        public static object o = new object();


        public static IConnection GetConnectionByServerInfo(string serverIp, int serverPort, string userName, string password)
        {
            lock (o)
            {
                IConnectionResource res = IConnectionResourceList.FirstOrDefault(x => x.ServerIp.Equals(serverIp) && x.ServerPort == serverPort);
                if (res == null)
                {
                    ConnectionFactory factory = new ConnectionFactory
                    {
                        Port = serverPort,
                        UserName = userName,
                        Password = password,
                        AutomaticRecoveryEnabled = true,    //自动重连
                        TopologyRecoveryEnabled = true,     //恢复拓扑结构
                        UseBackgroundThreadsForIO = true,   //后台处理消息
                        RequestedHeartbeat = 60             //心跳超时时间
                    };

                    //支持集群扩展,集群主机IP地址以逗号','分隔
                    IConnection connection = null;
                    if (serverIp.Contains(","))
                    {
                        List<string> hosts = new List<string>();
                        serverIp.Split(',').ToList().ForEach(host =>
                        {
                            if (!string.IsNullOrWhiteSpace(host))
                                hosts.Add(host);
                        });
                        connection = factory.CreateConnection(hosts);
                    }
                    else
                    {
                        factory.HostName = serverIp;
                        connection = factory.CreateConnection();
                    }

                    connection.ConnectionShutdown += connection_ConnectionShutdown;
                    connection.CallbackException += connection_CallbackException;
                    connection.ConnectionBlocked += connection_ConnectionBlocked;
                    connection.ConnectionUnblocked += connection_ConnectionUnblocked;

                    res = new IConnectionResource()
                    {
                        ServerIp = serverIp,
                        ServerPort = serverPort,
                        Connection = connection,
                        ReferenceCount = 1
                    };
                    IConnectionResourceList.Add(res);
                }
                else
                {
                    res.ReferenceCount += 1;
                }
                //
                return res.Connection;
            }
        }


        #region IConnection事件

        private static void connection_ConnectionUnblocked(object sender, EventArgs e)
        {
            //TextLog.SaveError(e.ToString());
        }

        private static void connection_ConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            //TextLog.SaveError(e.Reason);
        }

        private static void connection_CallbackException(object sender, CallbackExceptionEventArgs e)
        {
            //TextLog.SaveError(e.Exception.Message);
        }

        private static void connection_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            //TextLog.SaveError(e.ReplyText);
        }

        #endregion


        #region Channel事件回调

        private void Channel_CallbackException(object sender, CallbackExceptionEventArgs e)
        {
            //TextLog.SaveError(e.Exception.Message);
        }

        private void Channel_BasicRecoverOk(object sender, EventArgs e)
        {
            //TextLog.SaveError(e.ToString());
        }

        private void Channel_ModelShutdown(object sender, ShutdownEventArgs e)
        {
            //TextLog.SaveError(e.ReplyText);
        }

        #endregion


        #region 消息订阅

        /// <summary>
        /// 订阅者注册
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
        /// <returns></returns>
        public bool Register_durable_Exchange_and_Queue(string serverIp, int serverPort, string userName, string password, string exchangeName, string queueName, string typeName = "topic", string routeKey = "", bool exchangeDurable = true, bool exchangeAutoDelete = false, bool queueDurable = true, bool queueAutoDelete = false, bool queueExclusive = false, ushort prefetchCount = 0, bool noAck = false, int maxLength = -1, int maxBytes = -1, int messageTtl = -1)
        {
            this.ServerIp = serverIp;
            this.ServerPort = serverPort;
            this.UserName = userName;
            this.Password = password;
            this.ExchangeName = exchangeName;
            this.QueueName = queueName;
            this.RoutingKey = queueName;
            this.TypeName = typeName;
            this.ExchangeDurable = exchangeDurable;
            this.ExchangeAutoDelete = exchangeAutoDelete;
            this.QueueDurable = queueDurable;
            this.QueueAutoDelete = queueAutoDelete;
            this.QueueExclusive = queueExclusive;
            this.RoutingKey = string.IsNullOrWhiteSpace(routeKey) ? ExchangeName + "." + QueueName : routeKey;
            this.NoAck = noAck;
            this.PrefetchCount = prefetchCount;
            this.IsSubscriber = true;
            this.QueueMessageTtl = messageTtl;
            this.QueueMaxLength = maxLength;
            this.QueueMaxBytes = maxBytes;

            Dictionary<string, object> dic = new Dictionary<string, object>();
            //
            if (this.QueueMaxLength > 1)
            {
                dic.Add("x-max-length", this.QueueMaxLength);
            }
            if (this.QueueMaxBytes > 1)
            {
                dic.Add("x-max-length-bytes", this.QueueMaxBytes);
            }
            if (this.QueueMessageTtl > 1000)
            {
                dic.Add("x-message-ttl", this.QueueMessageTtl);
            }

            bool success = true;
            try
            {
                connection = GetConnectionByServerInfo(this.ServerIp, this.ServerPort, this.UserName, this.Password);
                //
                channel = connection.CreateModel();
                channel.ModelShutdown += Channel_ModelShutdown;
                channel.BasicRecoverOk += Channel_BasicRecoverOk;
                channel.CallbackException += Channel_CallbackException;

                channel.ExchangeDeclare(
                    exchange: this.ExchangeName,        //Exchange的名称
                    type: this.TypeName,                //类型
                    durable: this.ExchangeDurable,      //持久化
                    autoDelete: this.ExchangeAutoDelete,//自动删除
                    arguments: null                     //
                    );
                channel.QueueDeclare(
                    queue: this.QueueName,              //队列名
                    durable: this.QueueDurable,         //持久化
                    exclusive: this.QueueExclusive,     //排他性,断线自动删除
                    autoDelete: this.QueueAutoDelete,   //自动删除
                    arguments: dic                      //扩展参数
                    );
                channel.QueueBind(
                    queue: this.QueueName,
                    exchange: this.ExchangeName,
                    routingKey: this.RoutingKey);

                eventintBasicConsumer = new EventingBasicConsumer(channel);
                eventintBasicConsumer.Received += eventintBasicConsumer_Received;
                //
                channel.BasicQos(0, this.PrefetchCount, false);
                channel.BasicConsume(this.QueueName, this.NoAck, eventintBasicConsumer);
            }
            catch (Exception ex)
            {
                success = false;
                throw ex;
            }
            return success;
        }

        /// <summary>
        /// 消息回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void eventintBasicConsumer_Received(object sender, BasicDeliverEventArgs e)
        {
            if (MessageCallback != null)
            {
                MessageCallback(this, e.DeliveryTag, e.Body);
            }
        }

        /// <summary>
        /// 消息应答
        /// </summary>
        /// <param name="deliveryTag"></param>
        /// <param name="multiple"></param>
        public bool AckAnswer(ulong deliveryTag, bool multiple = false)
        {
            try
            {
                channel.BasicAck(deliveryTag, multiple);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


        #region 消息发送

        /// <summary>
        /// 发送者注册
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
        public bool Register_durable_Exchange_and_Queue(string serverIp, int serverPort, string userName, string password, string exchangeName, string typeName = "topic", string routeKey = "", bool exchangeDurable = true, bool exchangeAutoDelete = false)
        {
            this.ServerIp = serverIp;
            this.ServerPort = serverPort;
            this.UserName = userName;
            this.Password = password;
            this.ExchangeName = exchangeName;
            this.RoutingKey = routeKey;
            this.TypeName = typeName;
            this.ExchangeDurable = exchangeDurable;
            this.ExchangeAutoDelete = exchangeAutoDelete;

            bool success = true;
            try
            {
                connection = GetConnectionByServerInfo(this.ServerIp, this.ServerPort, this.UserName, this.Password);
                //
                channel = connection.CreateModel();
                channel.ModelShutdown += Channel_ModelShutdown;
                channel.BasicRecoverOk += Channel_BasicRecoverOk;
                channel.CallbackException += Channel_CallbackException;

                channel.ExchangeDeclare(
                    exchange: this.ExchangeName,        //Exchange的名称
                    type: this.TypeName,                //类型
                    durable: this.ExchangeDurable,      //持久化
                    autoDelete: this.ExchangeAutoDelete,//自动删除
                    arguments: null                     //
                    );

                Props = channel.CreateBasicProperties();
                Props.Persistent = false;
            }
            catch (Exception ex)
            {
                success = false;
                throw ex;
            }
            return success;
        }

        /// <summary>
        /// 消息入队
        /// </summary>
        /// <param name="message">消息字符串</param>
        /// <param name="persistent">持久化</param>
        /// <returns></returns>
        public bool MessageEnqueue(string message, bool persistent = true)
        {
            try
            {
                Props.Persistent = persistent;

                var msgBody = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: this.ExchangeName, routingKey: this.RoutingKey, basicProperties: this.Props, body: msgBody);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 异步消息入队
        /// </summary>
        /// <param name="message">消息字符串</param>
        /// <param name="persistent">持久化</param>
        /// <returns></returns>
        public async Task<bool> MessageEnqueueAsync(string message, bool persistent = true)
        {
            return await Task<bool>.Run(() =>
            {
                return MessageEnqueue(message, persistent);
            });
        }

        #endregion

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (this.IsSubscriber)
                    eventintBasicConsumer.Received -= eventintBasicConsumer_Received;
                //
                channel.Close();
                channel.Dispose();
                //
                lock (o)
                {
                    IConnectionResource res = IConnectionResourceList.FirstOrDefault(x => x.ServerIp.Equals(this.ServerIp) && x.ServerPort == ServerPort);
                    if (res != null)
                    {
                        res.ReferenceCount--;
                        if (res.ReferenceCount < 1)
                        {
                            connection.Close();
                            connection.Dispose();
                            //
                            IConnectionResourceList.Remove(res);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    /// <summary>
    /// RabbitMQ连接资源定义
    /// </summary>
    public class IConnectionResource
    {
        /// <summary>
        /// 服务地址
        /// </summary>
        public string ServerIp { get; set; }

        /// <summary>
        /// 服务端口
        /// </summary>
        public int ServerPort { get; set; }

        /// <summary>
        /// 连接实例
        /// </summary>
        public IConnection Connection { get; set; }

        /// <summary>
        /// 引用计数
        /// </summary>
        public int ReferenceCount { get; set; }
    }
}
