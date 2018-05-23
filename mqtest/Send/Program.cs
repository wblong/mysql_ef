using System;
using RabbitMQ.Client;
using System.Text;
using System.Threading;
class Send
{
    public static void Main()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using(var connection = factory.CreateConnection())
        using(var channel = connection.CreateModel())
        {
            string message = "Hello World!";
            var body = Encoding.UTF8.GetBytes(message);
            while(true){
                channel.BasicPublish(exchange: "test",
                                 routingKey: "hello",
                                 basicProperties: null,
                                 body: body);
                Console.WriteLine(" [x] Sent {0}", message);
                Thread.Sleep(1000);
            }
            
        }

        Console.WriteLine(" Press [enter] to exit.");

        Console.ReadLine();
    }
}