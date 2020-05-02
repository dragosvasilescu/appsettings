using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Collections;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;
using Newtonsoft.Json;

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
            IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true, true)
            .Build();

            var children = config.GetSection("MailSettings").GetChildren()
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

            //
            Console.WriteLine("\n\n\n\n Write Queues");
            Console.WriteLine("________________________________");
            var wq = config.GetSection("WriteQueues").Get<Dictionary<string, QueueConnecitonSettings>>();

            foreach (var item in wq)
            {
                Console.WriteLine($"{item.Key} - {JsonConvert.SerializeObject(item.Value)} ");
            }
            Console.ReadKey();
        }
    }
}
