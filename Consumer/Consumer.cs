using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

using SyslogLogger;
using DataLayer;
using TransportLayer;


namespace Consumer
{
    class Program
    {
        private const string  HOSTNAME = "syslog";
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
                .ConfigureServices((_, services) =>
                {
                    services.AddDbContext<MessagingContext>(
                        dbContextOptions => {
                            dbContextOptions
                            .UseMySql(
                                // Replace with your connection string.
                                "server=sqlserver;user=user;password=password;Database=ef",
                                // Replace with your server version and type.
                                // For common usages, see pull request #1233.
                                new MySqlServerVersion(new Version(8, 0, 21)), // use MariaDbServerVersion for MariaDB
                                mySqlOptions => mySqlOptions.CharSetBehavior(CharSetBehavior.NeverAppend)
                            );
                        },
                        ServiceLifetime.Transient,
                        ServiceLifetime.Transient
                    );
                    services.AddHostedService<ConsumerWorker>();
                    services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
                    services.AddScoped<IMessageQueue, MessageQueue>();
                    services.AddScoped<IShouldStoreStrategy, SecondsEvenShouldStoreStrategy>();
                });
    }
}

