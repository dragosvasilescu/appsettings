using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AppSettingsConsole.Services
{
    public class SampleService1 : BackgroundService
    {
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"Start {nameof(SampleService1)}...");
            return base.StartAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine($"running {nameof(SampleService1)}");
                await Task.Delay(1000, stoppingToken);
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Stop Sample...");
            return base.StopAsync(cancellationToken);
        }
    }
}
