using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using T.MessageQueue.Rabbit;

namespace T.MessageQueue.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Rabbit MQ测试控制台";

            Console.WriteLine("****** 输入数字1以发布者方式运行，输入数字2以订阅者方式运行 ******");
            Console.Write("输入数字 1 或 2：");
            int cmd;
            if (!int.TryParse(Console.ReadKey().KeyChar.ToString(), out  cmd))
            {
                Console.WriteLine("输入错误，按任意键退出!");
                Console.ReadLine();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\r\n\r\n开始\r\n");
            Console.ForegroundColor = ConsoleColor.White;
            string txt = File.ReadAllText("test.xml");

            MessageQueueManager.InitMessageQueueServer("192.168.5.16", 5672, "trkj", "trkj");

            switch (cmd)
            {
                case 1:
                    Console.Title = "Rabbit MQ 发布者";

                    IQueuePublisher publisher = MessageQueueManager.GerarateIQueuePublisher(
                        exchangeName: "my_test_exchange",
                        typeName: "topic",
                        routeKey: "test_key",
                        exchangeDurable: true,
                        exchangeAutoDelete: false);
                    //消息进队
                    for (int i = 0; i < int.MaxValue; i++)
                    {
                        Console.WriteLine(i);
                        publisher.MessageEnqueue(txt, true);
                        Thread.Sleep(1000);
                    }
                    break;

                case 2:
                    Console.Title = "Rabbit MQ 订阅者";

                    IQueueSubscriber subscriber = MessageQueueManager.GerarateIQueueSubscriber(
                        exchangeName: "my_test_exchange",
                        queueName: "my_test_queue",
                        typeName: "topic",
                        routeKey: "test_key",
                        exchangeDurable: true,
                        exchangeAutoDelete: false,
                        queueDurable: true,
                        queueAutoDelete: false,
                        queueExclusive: false,
                        prefetchCount: 0,
                        noAck: false,
                        maxLength: -1,
                        maxBytes: -1,
                        messageTtl: -1);
                    subscriber.MessageCallback += Subscriber_MessageCallback;
                    break;
                default:
                    Console.WriteLine("输入错误，按任意键退出!");
                    Console.ReadLine();
                    break;
            }

            Console.ReadLine();
        }

        private static void Subscriber_MessageCallback(IQueueSubscriber sender, ulong deliveryTag, byte[] body)
        {
            //收到信息输出
            string msg = Encoding.Default.GetString(body);

            //回复确认消息
            sender.AckAnswer(deliveryTag);

            Console.WriteLine(msg);
        }
    }
}
