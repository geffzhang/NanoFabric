using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleService.Kestrel
{
    public class AppSettings
    {
        public Consul Consul { get; set; }
    }

    public class Consul
    {
        public string HostName { get; set; }
        public int? Port { get; set; }
    }
}
