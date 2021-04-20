using System;
using System.Text;
using RabbitMQ.Client;

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using TransportLayer;
using Models;

namespace producer
{
    class ProducerWorker : BackgroundService
    {
        private readonly IMessageQueue queue;
        ILogger<ProducerWorker> _logger;
        public ProducerWorker(IMessageQueue queue, ILogger<ProducerWorker> logger)
        {
            this.queue = queue;
            this._logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(async () =>
            {
                while(true) {
                    queue.Publish(new Message());
                    await Task.Delay(500);
                } 
            });
        }
    }
}
