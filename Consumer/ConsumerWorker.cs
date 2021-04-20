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
        private readonly IShouldStoreStrategy storeStrategy;
        ILogger<ConsumerWorker> _logger;
        public ConsumerWorker(IMessageQueue queue, IShouldStoreStrategy storeStrategy, IRepository<Message> repository, ILogger<ConsumerWorker> logger)
        {
            this.queue = queue;
            this.storeStrategy = storeStrategy;
            this.repository = repository;
            this._logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(() => 
            {
                foreach (var message in queue.GetMessages())
                {
                    if (this.storeStrategy.ShouldStore(message)) {
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

}

