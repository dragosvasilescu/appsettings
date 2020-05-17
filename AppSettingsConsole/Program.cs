using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using AppSettingsConsole.Services;
using AppSettingsConsole.Models;
using log4net;
using System.Reflection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;

namespace ConsoleApp10
{
    

    class Program
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {
            var hostBuilder = new HostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureHostConfiguration(configurationBuilder =>
                {
                    configurationBuilder.AddCommandLine(args);
                })
                .ConfigureAppConfiguration((hostingContext, cfg) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    Console.WriteLine(env.EnvironmentName);

                    cfg.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    cfg.AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
                    var config = cfg.Build();
                })
                .ConfigureServices((hostingContext, services) =>
                {
                    MailSettings mailSettings = new MailSettings();
                    hostingContext.Configuration.GetSection("MailSettings").Bind(mailSettings);
                    services.AddHostedService<HostedService1>();
                    services.AddHostedService<HostedService2>(); 

                    services.Configure<MailSettings>(hostingContext.Configuration.GetSection("MailSettings"));
                    services.Configure<Options>(hostingContext.Configuration);
                    services.AddSingleton(hostingContext.Configuration);

                    services.AddLogging(configure =>
                    {
                        configure.ClearProviders();
                        configure.AddLog4Net("log4net.config");
                    });

                    services.AddSingleton<IOperationSingleton, Operation>();
                    services.AddSingleton<IOperationSingletonInstance>(new Operation(Guid.Parse("5d47dbc5-775b-48a8-9409-ff73379e4793")));
                    services.AddTransient<IOperationTransient, Operation>();
                    services.AddScoped<IOperationScoped, Operation>();
                });

            hostBuilder.RunConsoleAsync().Wait();
          
        }

        private static void Numbers_ItemDequeued(object sender, int e)
        {
            Console.WriteLine($"{e} has been dequeued");
        }

        private static void Numbers_ItemEnqueued(object sender, EventArgs e)
        {
           
        }
    }
}
