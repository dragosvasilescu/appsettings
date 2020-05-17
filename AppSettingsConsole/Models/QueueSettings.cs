using System;
using System.Collections.Generic;
using System.Text;

namespace AppSettingsConsole.Models
{
    public class QueueSettings
    {
        public string HostName { get; set; }
        public string Password { get; set; }
        public bool SSL { get; set; }
    }
}
