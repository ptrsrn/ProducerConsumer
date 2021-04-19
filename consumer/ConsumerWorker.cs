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
    class ConsumerWorker : BackgroundService
    {
        /* not DRY*/
        private const string BROKER = "rabbitmq";
        private const int PORT = 5672;
        private const string QUEUE = "task_queue";
        ILogger<ConsumerWorker> _logger;
        public ConsumerWorker(ILogger<ConsumerWorker> logger)
        {
            this._logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(() => 
            {
                this._logger.LogInformation("Broker: {0}:{1} using Queue: {2}", BROKER, PORT, QUEUE);
                var factory = new ConnectionFactory() { HostName = BROKER, Port = PORT };
                using(var connection = factory.CreateConnection())
                {
                    using(var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: QUEUE,
                                            durable: true,
                                            exclusive: false,
                                            autoDelete: false,
                                            arguments: null);

                        var consumer = new EventingBasicConsumer(channel);

                        //message received callback
                        consumer.Received += (model, ea) =>
                        {
                            this._logger.LogInformation("Message received: {0} sendt @ {1}", ea.DeliveryTag, ea.BasicProperties.Timestamp.ToString());
                            channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                        };

                        
                        while(true) {
                            channel.BasicConsume(queue: QUEUE,
                            autoAck: false, // do not remove the message from the broker before we have processed it.
                            consumer: consumer);
                        }

                    }
                }
            });
            
        }
    }
}

