using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using SyslogLogger;

namespace producer
{
    class Program
    {
        private const string HOSTNAME = "syslog";
        private const int PORT = 514;

        static Task Main(string[] args) =>
        CreateHostBuilder(args).Build().RunAsync();

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(builder =>
                {
                    builder.ClearProviders()
                    .AddSyslog(HOSTNAME, PORT);
                })
                .ConfigureServices((_, services) => {
                    services.AddHostedService<ProducerWorker>();}
                );
    }
}
