using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace NanoFabric.Grpc.Client
{
    /// <summary>
    /// GRpc <see cref="Channel"/> Channel Factory
    /// </summary>
    public interface IGrpcChannelFactory
    {
        /// <summary>
        /// Create a new <see cref=""Channel"/">object</see> based on the GRPC server address provided
        /// </summary>
        /// <param name="address">GRpc server address</param>
        /// <param name="port">GRpc server port</param>
        /// <returns></returns>
        Channel Get(string address, int port);
    }
}
