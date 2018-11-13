using System;
using System.Collections.Generic;
using System.Text;

namespace NanoFabric.Exceptionless.Model
{
    /// <summary>
    /// 扩展数据
    /// </summary>
    public class ExcDataParam
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 数据内容
        /// </summary>
        public object Data { get; set; }
    }
}
