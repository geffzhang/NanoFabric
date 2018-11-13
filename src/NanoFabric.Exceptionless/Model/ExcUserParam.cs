using System;
using System.Collections.Generic;
using System.Text;

namespace NanoFabric.Exceptionless.Model
{
    /// <summary>
    /// 用户标识参数
    /// </summary>
    public class ExcUserParam
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 邮件地址
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 描述信息
        /// </summary>
        public string Description { get; set; }
    }
}
