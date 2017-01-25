using System.Collections.Generic;

namespace CFT.NanoFabric.Core
{
    public class RegistryInformation
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Address { get; set; }
        public int Port { get; set; }
        public string Version { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}
