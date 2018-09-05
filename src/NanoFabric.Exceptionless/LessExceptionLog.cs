using NanoFabric.Exceptionless.Extensions;
using NanoFabric.Exceptionless.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NanoFabric.Exceptionless
{
    public class LessExceptionLog : ILessExceptionLog
    {
        /// <summary>
        /// 提交异常
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="tags">标签</param>
        public void Submit(string message, params string[] tags)
        {
            Submit(message, data: null, tags: tags);
        }

        /// <summary>
        /// 提交异常
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户</param>
        /// <param name="tags">标签</param>
        public void Submit(string message, ExcUserParam user, params string[] tags)
        {
            Submit(message, user, data: null, tags: tags);
        }

        /// <summary>
        /// 提交异常
        /// </summary>
        /// <param name="message">特性信息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        public void Submit(string message, ExcDataParam data, params string[] tags)
        {
            Submit(message, null, data, tags);
        }

        /// <summary>
        /// 提交异常
        /// </summary>
        /// <param name="message">特性信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        public void Submit(string message, List<ExcDataParam> datas, params string[] tags)
        {
            Submit(message, null, datas, tags);
        }

        /// <summary>
        /// 提交异常
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        public void Submit(string message, ExcUserParam user, ExcDataParam data, params string[] tags)
        {
            var datas = new List<ExcDataParam>() { data };
            Submit(message, user, datas, tags);
        }

        /// <summary>
        /// 提交异常
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="user">用户信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        public void Submit(string message, ExcUserParam user, List<ExcDataParam> datas, params string[] tags)
        {
            var stackTraceList = GetStackTraceList();
            var innerMsg = string.Join("", stackTraceList);
            var innerException = new Exception(innerMsg);
            var ex = new Exception(message, innerException);
            ex.Submit(user, datas, tags);
        }

        /// <summary>
        /// 获取自定义堆栈信息
        /// </summary>
        /// <param name="traceList"></param>
        /// <returns></returns>
        private static IEnumerable<string> GetStackTraceList()
        {
            var traceStringList = GetStackTraceStringList();
            foreach (var trace in traceStringList)
            {
                string traceTrimStr = trace?.Trim();
                if (!string.IsNullOrEmpty(traceTrimStr) && trace.LastIndexOf(@"NanoFabric.Exceptionless\LessExceptionLog.cs") < 0
                    && trace.LastIndexOf(@"System.String.Join") < 0)
                {
                    yield return $"   at {traceTrimStr}\r\n";
                }
            }
            yield break;
        }

        /// <summary>
        /// 获取当前堆栈的字符串数组
        /// </summary>
        /// <returns></returns>
        private static string[] GetStackTraceStringList()
        {
            String[] separator = new string[] { "  at " };
            var traceStrings = new StackTrace(true).ToString();
            return traceStrings.Split(separator, StringSplitOptions.None);
        }
    }
}
