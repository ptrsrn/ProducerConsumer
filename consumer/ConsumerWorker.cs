using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.Concurrent;

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using Entityframework;

namespace consumer
{
    interface IRepository<TEntity> where TEntity: class, new()
    {   
        TEntity Add(TEntity entity);
    }

    class Repository<TEntry> : IRepository<TEntry> where TEntry: class, new()
    {
        private readonly MessagingContext context;

        public Repository(MessagingContext context)
        {
            this.context = context;
        }

        public TEntry Add(TEntry entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity must not be null");
            }

            try
            {

                this.context.Add(entity);
                this.context.SaveChanges();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be saved: {ex.Message}");
            }
        }
    }

    class ConsumerWorker : BackgroundService
    {
        private readonly consumer.IRepository<Message> repository;
        private readonly consumer.IMessageQueue queue;
        ILogger<ConsumerWorker> _logger;
        public ConsumerWorker(IMessageQueue queue, IRepository<Message> repository, ILogger<ConsumerWorker> logger)
        {
            this.queue = queue;
            this.repository = repository;
            this._logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(() => 
            {
                foreach (var message in queue.GetMessages())
                {
                    if (message.Received.Ticks%2 == 0) {
                        this._logger.LogInformation("Message {0}  satisfy condition", message.DeliveryTag);
                        this.repository.Add(message);
                    }
                    else
                    {
                        this._logger.LogInformation("Message {0} does not satisfy condition", message.DeliveryTag);
                        queue.Publish(message);
                    }
                    queue.Remove(message);
                }
            });
            
        }
    }



    interface IMessageQueue
    {
        void Publish(Message message);
        void Remove(Message message);
        IEnumerable<Message> GetMessages();
    }

    class MessageQueue : IMessageQueue
    {
        
        private const string BROKER = "rabbitmq";
        private const int PORT = 5672;
        private const string QUEUE = "task_queue";

        private ConnectionFactory factory;
        private IConnection connection;
        private IModel channel;
        private ILogger<MessageQueue> logger;


        public MessageQueue(ILogger<MessageQueue> logger) 
        {
            this.logger = logger;
            this.logger.LogInformation("Broker: {0}:{1} using Queue: {2}", BROKER, PORT, QUEUE);
            this.factory = new ConnectionFactory() { HostName = BROKER, Port = PORT };
            this.connection = factory.CreateConnection();
            this.channel = connection.CreateModel();
            channel.QueueDeclare(queue: QUEUE,
                                            durable: true,
                                            exclusive: false,
                                            autoDelete: false,
                                            arguments: null);
        }

        public void Publish(Message message)
        {
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.Timestamp = new AmqpTimestamp(DateTime.UtcNow.Ticks);

            channel.BasicPublish(exchange: "",
                                routingKey: QUEUE,
                                basicProperties: properties,
                                body: Encoding.UTF8.GetBytes("Hello World!"));
        }

        public void Remove(Message message){
            channel.BasicAck(deliveryTag:  Convert.ToUInt64(message.DeliveryTag), multiple: false);
        }

        public IEnumerable<Message> GetMessages() {
            using(var resultsQueue = new BlockingCollection<Message>()) {
                var consumer = new EventingBasicConsumer(channel);

                //message received callback
                consumer.Received += (model, ea) => {
                    Message message = new Message(){ Sent = new DateTime( ea.BasicProperties.Timestamp.UnixTime).ToUniversalTime(), Received = DateTime.UtcNow, DeliveryTag = ea.DeliveryTag.ToString() };
                    this.logger.LogInformation("Message {0} received @ {1}", message.DeliveryTag, message.Received);
                    resultsQueue.Add(message);
                };

                channel.BasicConsume(queue: QUEUE,
                        autoAck: false, // do not remove the message from the broker before we have processed it.
                        consumer: consumer);

                while(true) {
                    yield return resultsQueue.Take();
                }

            }
        }

    }


}

