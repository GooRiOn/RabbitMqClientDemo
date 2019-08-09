using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost", VirtualHost = "/"};
            using(var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDelete("hello");
                channel.ExchangeDeclare(exchange: "hello",
                    durable: true,
                    autoDelete: false,
                    arguments: null,
                    type: "direct");
                

                channel.QueueDelete("hello.queue");
                channel.QueueDeclare(queue: "hello.queue",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
                
                channel.QueueBind("hello.queue", "hello", "#.hello.#");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received {0}", message);
                };
                channel.BasicConsume(queue: "hello.queue",
                    autoAck: true,
                    consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}