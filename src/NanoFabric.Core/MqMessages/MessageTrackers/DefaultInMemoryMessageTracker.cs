using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoFabric.Core.MqMessages.MessageTrackers
{
    public class DefaultInMemoryMessageTracker : IMessageTracker
    {
        private static readonly ConcurrentBag<string> InMemoryStore;

        static DefaultInMemoryMessageTracker()
        {
            InMemoryStore = new ConcurrentBag<string>();
        }

        public static DefaultInMemoryMessageTracker Instance { get; } = new DefaultInMemoryMessageTracker();

        public Task MarkAsProcessed(string processId)
        {
            InMemoryStore.Add(processId);
            return Task.FromResult(0);
        }

        public Task<bool> HasProcessed(string processId)
        {
            return Task.FromResult(InMemoryStore.Contains(processId));
        }
    }
}
