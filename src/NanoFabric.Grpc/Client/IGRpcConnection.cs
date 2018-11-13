using MagicOnion;
using System.Threading.Tasks;

namespace NanoFabric.Grpc.Client
{
    public interface IGRpcConnection
    {
        /// <summary>
        /// Get the specified remote service interface
        /// </summary>
        ///<typeparam name="TService">Remote Service Interface type</typeparam>
        ///<param name = "serviceName"> Remote Service Name</param>
        Task<TService> GetRemoteService<TService>(string serviceName) where TService : IService<TService>;
    }
}
