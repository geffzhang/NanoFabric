using System;
using System.Collections.Generic;
using System.Text;
using Grpc.Core;

namespace NanoFabric.Grpc.Client
{
    public class GrpcChannelFactory : IGrpcChannelFactory
    {
        private static readonly object _syncLock = new object();
        private Dictionary<string, Channel> grpcServers;
 
        public GrpcChannelFactory()
        {
            grpcServers = new Dictionary<string, Channel>();
        }

        public Channel Get(string address, int port)
        {
            Channel channel = null;

            string key = $"{address}:{port}";
            if (!grpcServers.ContainsKey(key))
            {
                lock (_syncLock)
                {                    
                    channel = new Channel(address, port, ChannelCredentials.Insecure);
                    grpcServers.Add(key, channel);
                }
            }
            else
            {
                channel = grpcServers[key];
            }
            return channel;
        }
    }
}
