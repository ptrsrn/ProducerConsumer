using System;
using System.Text;

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace consumer
{
    class ConsumerWorker : BackgroundService {
        ILogger<ConsumerWorker> _logger;
        public ConsumerWorker(ILogger<ConsumerWorker> logger)
        {
            this._logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(() => 
            {
                String hostname = "rabbitmq"; //discoverable hostname inside the docker container

                Console.Out.WriteLine("Host: " + hostname );
                var factory = new ConnectionFactory() { HostName = hostname, Port = 5672 };
                using(var connection = factory.CreateConnection())
                {
                    using(var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: "task_queue",
                                            durable: true,
                                            exclusive: false,
                                            autoDelete: false,
                                            arguments: null);

                        var consumer = new EventingBasicConsumer(channel);

                        consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body.ToArray();
                            var message = Encoding.UTF8.GetString(body);
                            Console.WriteLine(" [x] Received {0}", message);
                            

                            channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                        };


                        channel.BasicConsume(queue: "hello",
                                            autoAck: false, // do not remove the message from the broker before we have processed it.
                                            consumer: consumer);

                        Console.WriteLine(" Press [enter] to exit.");
                        Console.ReadLine();
                    }
                }
            });
            
        }
    }
}

