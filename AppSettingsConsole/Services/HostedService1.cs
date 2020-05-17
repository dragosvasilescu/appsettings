using AppSettingsConsole.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace AppSettingsConsole.Services
{
    public class HostedService1 : BackgroundService
    {
        ILogger<HostedService1> Logger;
        MailSettings MailSettings;
        Models.Options Options;
        IOperationSingleton Singleton;
        IOperationSingletonInstance SingletonInstance;
        IOperationTransient TransientInstance;
        IServiceProvider ServiceProvider;

        void f1()
        {
            var sc1 = ServiceProvider.GetRequiredService<IOperationScoped>();
            Console.WriteLine(sc1.OperationId);
        }

        void f2()
        {
            var sc2 = ServiceProvider.GetRequiredService<IOperationScoped>();
            Console.WriteLine(sc2.OperationId);
        }

        public HostedService1(ILogger<HostedService1> logger, IOptionsMonitor<MailSettings> mailSettingsAccessor, 
            IOptionsMonitor<Models.Options> options, IOperationSingleton singleton, IOperationSingletonInstance singletonInstance, IOperationTransient transientInstance, IServiceProvider serviceProvider)
        {
            Logger = logger;
            MailSettings = mailSettingsAccessor.CurrentValue;
            Options = options.CurrentValue;
            Singleton = singleton;
            SingletonInstance = singletonInstance;
            TransientInstance = transientInstance;
            ServiceProvider = serviceProvider;

           

            //Console.WriteLine($"{nameof(HostedService1)} singleton guid is {Singleton.OperationId}");
            //Console.WriteLine($"{nameof(HostedService1)} singleton guid is {singletonInstance.OperationId}");
            //Console.WriteLine($"{nameof(HostedService1)} transient guid is {TransientInstance.OperationId}");
        }

        FileSystemWatcher watcher; 

        public override Task StartAsync(CancellationToken cancellationToken)
        {

            //f1();
            //f2();

            using (var scope = new TransactionScope())
            {
                scope.Complete();
            }

            using (var scope = ServiceProvider.CreateScope())
            {
                var sc11 = scope.ServiceProvider.GetRequiredService<IOperationScoped>();
                var sc22 = scope.ServiceProvider.GetRequiredService<IOperationScoped>();

                Console.WriteLine(sc11.OperationId);
                Console.WriteLine(sc22.OperationId);

            } 

            watcher = new FileSystemWatcher();
            watcher.Path = @"F:\";
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                   | NotifyFilters.FileName | NotifyFilters.DirectoryName; 

            watcher.Filter = "*.*";
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += Watcher_Created;
            watcher.EnableRaisingEvents = true;

            return base.StartAsync(cancellationToken);
        }

        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            Logger.LogError("Aceasta este o eroare!!!!"); 
            Logger.LogInformation("file created...");
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            Logger.LogInformation("file changesd...");
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation($"{nameof(HostedService1)} stopped...");
            return base.StopAsync(cancellationToken);
        }
    }
}
