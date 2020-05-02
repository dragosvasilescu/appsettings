using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AppSettingsConsole.Services
{
    public class SampleService2 : BackgroundService
    {
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"Start {nameof(SampleService2)}...");
            return base.StartAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine($"running {nameof(SampleService2)}");
                await Task.Delay(1000, stoppingToken);
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"Stop {nameof(SampleService2)}");
            return base.StopAsync(cancellationToken);
        }
    }
}
