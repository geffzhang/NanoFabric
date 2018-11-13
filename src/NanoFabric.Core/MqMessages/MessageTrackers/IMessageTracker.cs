using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NanoFabric.Core.MqMessages.MessageTrackers
{
    public interface IMessageTracker
    {
        /// <summary>
        /// 查询是否已处理过
        /// </summary>
        /// <param name="processId">能唯一标记本次处理过程的Id，可采用msgId+HandlerName等组合</param>
        /// <returns></returns>
        Task<bool> HasProcessed(string processId);

        /// <summary>
        /// 标记为已处理过
        /// </summary>
        /// <param name="processId">能唯一标记本次处理过程的Id，可采用msgId+HandlerName等组合</param>
        /// <returns></returns>
        Task MarkAsProcessed(string processId);
    }
}
