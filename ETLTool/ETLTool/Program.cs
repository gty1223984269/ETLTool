namespace ETLTool
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
                  .AddJsonFile("appsettings.cn.json", true);
                Configuration = builder.Build();
                var servicesProvider = BuildDi();
                var migrateService = servicesProvider.GetRequiredService<MigrateService>();
                var repositoryService = servicesProvider.GetRequiredService<Repository>();
                migrateService.Migrate(repositoryService);
            }

            public static IConfiguration Configuration { get; set; }

            private static IServiceProvider BuildDi()
            {
                var services = new ServiceCollection();
                services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));
                services.Configure<Endpoint>(Configuration.GetSection("Endpoint"));
                services.AddSingleton<ILoggerFactory, LoggerFactory>();
                services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
                services.AddLogging((builder) => builder.SetMinimumLevel(LogLevel.Trace));
                services.AddSingleton<MigrateService, MigrateService>();
                services.AddSingleton<Repository, Repository>();

                var serviceProvider = services.BuildServiceProvider();

                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

                loggerFactory.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });
                loggerFactory.ConfigureNLog("nlog.config");

                return serviceProvider;
            }
        }
    }
}