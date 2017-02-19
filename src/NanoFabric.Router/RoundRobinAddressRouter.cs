using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NanoFabric.Core;

namespace NanoFabric.Router
{
    /// <summary>
    /// 主机地址的轮播负载均衡
    /// </summary>
    public class RoundRobinAddressRouter : BaseLoadbalancingRouter<RegistryInformation>
    {
        private readonly Func<RegistryInformation, RegistryInformation, bool> _discriminator;

        public RoundRobinAddressRouter()
        {
            _discriminator = (x, y) => x?.Address == y?.Address;
        }

        public override RegistryInformation Choose(RegistryInformation previous, IList<RegistryInformation> instances)
        {
            int previousIndex = instances.IndexOf(previous);

            var next = instances.Skip(previousIndex < 0 ? 0 : previousIndex + 1)
                .FirstOrDefault(x => _discriminator(x, previous) == false) ?? instances.FirstOrDefault();

            Previous = next;

            return next;
        }
    }
}
