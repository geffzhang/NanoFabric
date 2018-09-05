using NanoFabric.Exceptionless.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace NanoFabric.Exceptionless
{
    /// <summary>
    /// 使用特性日志
    /// </summary>
    public interface ILessFeatureLog
    {
        /// <summary>
        /// 提交特性
        /// </summary>
        /// <param name="feature">特性信息</param>
        /// <param name="tags">标签</param>
        void Submit(string feature, params string[] tags);

        /// <summary>
        /// 提交特性
        /// </summary>
        /// <param name="feature">消息</param>
        /// <param name="user">用户</param>
        /// <param name="tags">标签</param>
        void Submit(string feature, ExcUserParam user, params string[] tags);

        /// <summary>
        /// 提交特性
        /// </summary>
        /// <param name="feature">特性信息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        void Submit(string feature, ExcDataParam data, params string[] tags);

        /// <summary>
        /// 提交特性
        /// </summary>
        /// <param name="feature">特性信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        void Submit(string feature, List<ExcDataParam> datas, params string[] tags);

        /// <summary>
        /// 提交特性
        /// </summary>
        /// <param name="feature">特性消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        void Submit(string feature, ExcUserParam user, ExcDataParam data, params string[] tags);

        /// <summary>
        /// 提交特性
        /// </summary>
        /// <param name="feature">特性信息</param>
        /// <param name="user">用户信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        void Submit(string feature, ExcUserParam user, List<ExcDataParam> datas, params string[] tags);
    }
}
