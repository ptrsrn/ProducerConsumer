using System;
using Microsoft.Extensions.Logging;

/**
We follow the same pattern as other loggers including those that come with ASP.NET Core. 
Loggers are introduced to logger factory by AddSomething()
*/
public static class SyslogLoggerExtensions
{
    public static ILoggingBuilder AddSyslog(this ILoggingBuilder factory,
                                    string host, int port,
                                    Func<string, LogLevel, bool> filter = null)
    {
        factory.AddProvider(new SyslogLoggerProvider(host, port, filter));
        return factory;
    }   
}