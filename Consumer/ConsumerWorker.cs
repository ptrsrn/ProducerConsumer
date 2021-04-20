using System;
using System.Text;

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;



using DataLayer;
using TransportLayer;
using Models;

namespace Consumer
{
    class ConsumerWorker : BackgroundService
    {
        private readonly IRepository<Message> repository;
        private readonly IMessageQueue queue;
        private readonly IMessageStrategy messageStrategy;
        ILogger<ConsumerWorker> logger;
        public ConsumerWorker(IMessageQueue queue, IMessageStrategy messageStrategy, IRepository<Message> repository, ILogger<ConsumerWorker> logger)
        {
            this.queue = queue;
            this.messageStrategy = messageStrategy;
            this.repository = repository;
            this.logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(() => 
            {
                foreach (var message in queue.GetMessages())
                {
                    if (this.messageStrategy.ShouldProcess(message)) {
                        if (this.messageStrategy.ShouldStore(message))
                        {
                            this.logger.LogInformation("Message {0}  satisfy condition", message.DeliveryTag);
                            this.repository.Add(message);
                        }
                        else
                        {
                            this.logger.LogInformation("Message {0} does not satisfy condition", message.DeliveryTag);
                            queue.Publish(message);
                        }
                        
                    }
                    
                    queue.Remove(message);
                }
            });
            
        }
    }

}

