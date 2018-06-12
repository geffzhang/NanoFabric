using System;
using System.Collections.Generic;
using System.Text;
using Grpc.Core;

namespace NanoFabric.Grpc.Client
{
    public class GrpcChannelFactory : IGrpcChannelFactory
    {
        public Channel Get(string address, int port)
        {
            return new Channel(address, port, ChannelCredentials.Insecure);
        }
    }
}
