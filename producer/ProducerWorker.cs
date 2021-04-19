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
        private const string BROKER = "rabbitmq";
        private const int PORT = 5672;
        private const string QUEUE = "task_queue";
        ILogger<ProducerWorker> _logger;
        public ProducerWorker(ILogger<ProducerWorker> logger)
        {
            this._logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(async () =>
            {
                this._logger.LogInformation("Broker: {0}:{1} using Queue: {2}", BROKER, PORT, QUEUE);

                var factory = new ConnectionFactory() { HostName = BROKER, Port = PORT };
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: QUEUE,
                                            durable: true,
                                            exclusive: false,
                                            autoDelete: false,
                                            arguments: null);

                        
                        while(true) {

                            var properties = channel.CreateBasicProperties();
                            properties.Persistent = true;
                            properties.Timestamp = new AmqpTimestamp(DateTime.UtcNow.Ticks);

                            channel.BasicPublish(exchange: "",
                                                routingKey: QUEUE,
                                                basicProperties: properties,
                                                body: Encoding.UTF8.GetBytes("Hello World!"));

                            this._logger.LogInformation("Message sent @ {0}", properties.Timestamp.ToString());
                            await Task.Delay(500);
                        }
                    }
                }
            });
        }
    }
}
