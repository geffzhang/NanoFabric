using NanoFabric.Exceptionless.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace NanoFabric.Exceptionless
{
    public interface ILessExceptionLog
    {
        /// <summary>
        /// 提交异常
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="tags">标签</param>
        void Submit(string message, params string[] tags);

        /// <summary>
        /// 提交异常
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户</param>
        /// <param name="tags">标签</param>
        void Submit(string message, ExcUserParam user, params string[] tags);

        /// <summary>
        /// 提交异常
        /// </summary>
        /// <param name="message">特性信息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        void Submit(string message, ExcDataParam data, params string[] tags);

        /// <summary>
        /// 提交异常
        /// </summary>
        /// <param name="message">特性信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        void Submit(string message, List<ExcDataParam> datas, params string[] tags);

        /// <summary>
        /// 提交异常
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        void Submit(string message, ExcUserParam user, ExcDataParam data, params string[] tags);

        /// <summary>
        /// 提交异常
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="user">用户信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        void Submit(string message, ExcUserParam user, List<ExcDataParam> datas, params string[] tags);
    }
}
