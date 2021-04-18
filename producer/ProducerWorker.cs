using System;
using System.Text;
using RabbitMQ.Client;

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace producer
{
    class ProducerWorker : BackgroundService
    {
        ILogger<ProducerWorker> _logger;
        public ProducerWorker(ILogger<ProducerWorker> logger)
        {
            this._logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(() =>
            {
                String hostname = "rabbitmq"; //discoverable hostname inside the docker container
                
                this._logger.LogInformation("Host: {0}", hostname);

                var factory = new ConnectionFactory() { HostName = hostname, Port = 5672 };
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: "task_queue",
                                            durable: true,
                                            exclusive: false,
                                            autoDelete: false,
                                            arguments: null);

                        string message = "Hello World!";
                        var body = Encoding.UTF8.GetBytes(message);

                        var properties = channel.CreateBasicProperties();
                        properties.Persistent = true;
                        properties.Timestamp = new AmqpTimestamp(DateTime.UtcNow.Ticks); //use absolute time
                        properties.ContentEncoding = Encoding.UTF8.BodyName;


                        channel.BasicPublish(exchange: "",
                                            routingKey: "hello",
                                            basicProperties: properties,
                                            body: body);
                        Console.WriteLine(" [x] Sent {0}", message);
                    }

                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }
            });
        }
    }
}
