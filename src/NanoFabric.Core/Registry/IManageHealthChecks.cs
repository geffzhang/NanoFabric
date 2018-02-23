using System;
using System.Threading.Tasks;

namespace NanoFabric.Core
{
    /// <summary>
    /// 服务健康检查
    /// </summary>
    public interface IManageHealthChecks
    {
        /// <summary>
        /// 注册服务的健康检查
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <param name="serviceId">服务ID</param>
        /// <param name="checkUri">健康检查服务</param>
        /// <param name="interval">间隔时间</param>
        /// <param name="notes"></param>
        /// <returns></returns>
        Task<string> RegisterHealthCheckAsync(string serviceName, string serviceId, Uri checkUri, TimeSpan? interval = null, string notes = null);

        /// <summary>
        /// 注销实例的健康检查服务
        /// </summary>
        /// <param name="checkId">实例的健康检查标识Id</param>
        /// <returns></returns>
        Task<bool> DeregisterHealthCheckAsync(string checkId);
    }
}
