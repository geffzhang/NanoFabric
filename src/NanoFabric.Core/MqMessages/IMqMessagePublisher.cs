using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NanoFabric.Core.MqMessages
{
    /// <summary>
    /// 消息发布接口
    /// </summary>
    public interface IMqMessagePublisher  
    {
        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="mqMessages"></param>
        void Publish(object mqMessages);

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="mqMessages"></param>
        /// <returns></returns>
        Task PublishAsync(object mqMessages);
    }
}
