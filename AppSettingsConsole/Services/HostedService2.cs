using AppSettingsConsole.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AppSettingsConsole.Services
{
    public class HostedService2 : BackgroundService
    {

        ILogger<HostedService1> Logger;
        MailSettings MailSettings;
        IConfiguration Configuration;
        IOperationSingleton Singleton;
        IOperationSingletonInstance SingletonInstance;
        IOperationTransient TransientInstance;

        public HostedService2(ILogger<HostedService1> logger, IOptions<MailSettings> mailSettingsAccessor, IConfiguration configuration, IOperationSingleton singleton, IOperationSingletonInstance singletonInstance, IOperationTransient transientInstance)
        {
            Logger = logger;
            MailSettings = mailSettingsAccessor.Value;
            Configuration = configuration;
            Singleton = singleton;
            SingletonInstance = singletonInstance;
            TransientInstance = transientInstance;

            //Console.WriteLine($"{nameof(HostedService2)} singleton guid is {Singleton.OperationId}");
            //Console.WriteLine($"{nameof(HostedService2)} singleton guid is {SingletonInstance.OperationId}");
            //Console.WriteLine($"{nameof(HostedService2)} transient guid is {TransientInstance.OperationId}");
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            Logger.LogError("Aceasta este o mare eroare!!!!!!"); 
            Logger.LogInformation($"Start {nameof(HostedService2)}...");
            return base.StartAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Task t1 = Task.Factory.StartNew(() =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    //Thread.Sleep(1000);
                    Logger.LogDebug("Acesta este un log de debug"); 
                    Logger.LogInformation(MailSettings.From);

                }
               
            });

            Task t2 = Task.Factory.StartNew(() =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    //Thread.Sleep(1000);
                    Logger.LogInformation(MailSettings.Server);
                    Logger.LogInformation(Configuration.GetValue<string>("Username") + "-----------------------------------------");
                }
            });

           
               await Task.WhenAll(t1, t2).ContinueWith(_=>
               {
                   foreach (var ex in _.Exception.InnerExceptions)
                   {
                       Console.WriteLine(ex.Message);
                   }
               }, TaskContinuationOptions.OnlyOnFaulted);
            
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation($"Stop {nameof(HostedService2)}");
            return base.StopAsync(cancellationToken);
        }
    }
}
