﻿namespace ETLTool
{
    using ETLTool.DataModel;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using NLog.Extensions.Logging;
    using System;

    namespace AvatorMigrate
    {
        internal class Program
        {
            private static void Main(string[] args)
            {
                var builder = new ConfigurationBuilder()
                  .SetBasePath(Environment.CurrentDirectory)
                  .AddJsonFile($"appsettings.{"cn"}.json", true);
                Configuration = builder.Build();
                var servicesProvider = BuildDi();
                var migrateService = servicesProvider.GetRequiredService<MigrateService>();
                migrateService.Migrate();
            }

            public static IConfiguration Configuration { get; set; }

            private static IServiceProvider BuildDi()
            {
                var services = new ServiceCollection();
                services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));
                services.Configure<TestModel>(Configuration.GetSection("TestModel"));
                services.AddSingleton<ILoggerFactory, LoggerFactory>();
                services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
                services.AddLogging((builder) => builder.SetMinimumLevel(LogLevel.Trace));
                services.AddSingleton<MigrateService, MigrateService>();
                var serviceProvider = services.BuildServiceProvider();

                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

                loggerFactory.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });
                loggerFactory.ConfigureNLog("nlog.config");

                return serviceProvider;
            }
        }
    }
}