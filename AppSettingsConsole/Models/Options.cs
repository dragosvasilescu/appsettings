using System;
using System.Collections.Generic;
using System.Text;

namespace AppSettingsConsole.Models
{
    public class Options
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public MailSettings MailSettings { get; set; }
        public List<string> BrowseQueues { get; set; }
        public Dictionary<string, QueueSettings> WriteQueues { get; set; }
    }
}