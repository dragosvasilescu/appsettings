using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using AppSettingsConsole.Services;

namespace ConsoleApp10
{
    public class QueueConnecitonSettings 
    {
        public string HostName { get; set; }
        public string Password { get; set; }
        public string SSL { get; set; }
    } 

    class Program
    {
        public Dictionary<string, object> MailSettings { get; set; }
        static void Main(string[] args)
        {
            if (args != null)
            {
                foreach (var item in args)
                {
                    Console.WriteLine(item);
                }
            }
            Console.WriteLine("-----------------------------");

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

                    cfg.AddJsonFile("appsettings.json", optional: false);
                    cfg.AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true);
                    var config = cfg.Build();

                    var children = hostingContext.Configuration.GetSection("MailSettings").GetChildren()
                .ToDictionary(x => x.Key, x => x.Value);

                    foreach (var item in children)
                    {
                        Console.WriteLine($"{item.Key} - {item.Value}");
                    }

                    var qq = config.GetSection("BrowseQueues").Get<List<string>>();

                    foreach (var item in qq)
                    {
                        Console.WriteLine(item);
                    }
                    Console.WriteLine("\n\n\n\n Write Queues");
                    Console.WriteLine("________________________________");
                    var wq = config.GetSection("WriteQueues").Get<Dictionary<string, QueueConnecitonSettings>>();
                    foreach (var item in wq)
                    {
                        Console.WriteLine($"{item.Key} - {JsonConvert.SerializeObject(item.Value)} ");
                    }
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<SampleService1>();
                    services.AddHostedService<SampleService2>();
                });
            
            
            hostBuilder.RunConsoleAsync().Wait();
          
        }
    }
}
