using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing;

namespace Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };
            
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
               

                string message = "Hello World!";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "hello",
                    routingKey: "#.hello.#",
                    basicProperties: new BasicProperties
                    {
                        Headers = new Dictionary<string, object>
                        {
                            {"sdsd", "sdasdasd"}
                        }
                    },
                    body: body);
                Console.WriteLine(" [x] Sent {0}", message);
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}