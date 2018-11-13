using Exceptionless;
using Exceptionless.Logging;
using NanoFabric.Exceptionless.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace NanoFabric.Exceptionless
{
    public class LessLog : ILessLog
    {
        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="tags">标签</param>
        public void Info(string message, params string[] tags)
        {
            Submit(message, LogLevel.Info, tags);
        }

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="tags">标签</param>
        public void Info(string message, ExcUserParam user, params string[] tags)
        {
            Submit(message, LogLevel.Info, user, tags);
        }

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        public void Info(string message, ExcDataParam data, params string[] tags)
        {
            Submit(message, LogLevel.Info, data, tags);
        }

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        public void Info(string message, List<ExcDataParam> datas, params string[] tags)
        {
            Submit(message, LogLevel.Info, datas, tags);
        }

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        public void Info(string message, ExcUserParam user, ExcDataParam data, params string[] tags)
        {
            Submit(message, LogLevel.Info, user, data, tags);
        }

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        public void Info(string message, ExcUserParam user, List<ExcDataParam> datas, params string[] tags)
        {
            Submit(message, LogLevel.Info, user, datas, tags);
        }

        /// <summary>
        /// 异常消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="tags">标签</param>
        public void Error(string message, params string[] tags)
        {
            Submit(message, LogLevel.Error, tags);
        }

        /// <summary>
        /// 异常消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="tags">标签</param>
        public void Error(string message, ExcUserParam user, params string[] tags)
        {
            Submit(message, LogLevel.Error, user, tags);
        }

        /// <summary>
        /// 异常消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        public void Error(string message, ExcDataParam data, params string[] tags)
        {
            Submit(message, LogLevel.Error, data, tags);
        }

        /// <summary>
        /// 异常消息
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        public void Error(string message, List<ExcDataParam> datas, params string[] tags)
        {
            Submit(message, LogLevel.Error, datas, tags);
        }

        /// <summary>
        /// 异常消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        public void Error(string message, ExcUserParam user, ExcDataParam data, params string[] tags)
        {
            Submit(message, LogLevel.Error, user, data, tags);
        }

        /// <summary>
        /// 异常消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        public void Error(string message, ExcUserParam user, List<ExcDataParam> datas, params string[] tags)
        {
            Submit(message, LogLevel.Error, user, datas, tags);
        }

        /// <summary>
        /// 跟踪信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="tags">标签</param>
        public void Trace(string message, params string[] tags)
        {
            Submit(message, LogLevel.Trace, tags);
        }

        /// <summary>
        /// 跟踪信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="tags">标签</param>
        public void Trace(string message, ExcUserParam user, params string[] tags)
        {
            Submit(message, LogLevel.Trace, user, tags);
        }

        /// <summary>
        /// 跟踪信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        public void Trace(string message, ExcDataParam data, params string[] tags)
        {
            Submit(message, LogLevel.Trace, data, tags);
        }

        /// <summary>
        /// 跟踪信息
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        public void Trace(string message, List<ExcDataParam> datas, params string[] tags)
        {
            Submit(message, LogLevel.Trace, datas, tags);
        }

        /// <summary>
        /// 跟踪信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        public void Trace(string message, ExcUserParam user, ExcDataParam data, params string[] tags)
        {
            Submit(message, LogLevel.Trace, user, data, tags);
        }

        /// <summary>
        /// 跟踪信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        public void Trace(string message, ExcUserParam user, List<ExcDataParam> datas, params string[] tags)
        {
            Submit(message, LogLevel.Trace, user, datas, tags);
        }

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="tags">标签</param>
        public void Debug(string message, params string[] tags)
        {
            Submit(message, LogLevel.Debug, tags);
        }

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="tags">标签</param>
        public void Debug(string message, ExcUserParam user, params string[] tags)
        {
            Submit(message, LogLevel.Debug, user, tags);
        }

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        public void Debug(string message, ExcDataParam data, params string[] tags)
        {
            Submit(message, LogLevel.Debug, data, tags);
        }

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        public void Debug(string message, List<ExcDataParam> datas, params string[] tags)
        {
            Submit(message, LogLevel.Debug, datas, tags);
        }

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        public void Debug(string message, ExcUserParam user, ExcDataParam data, params string[] tags)
        {
            Submit(message, LogLevel.Debug, user, data, tags);
        }

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        public void Debug(string message, ExcUserParam user, List<ExcDataParam> datas, params string[] tags)
        {
            Submit(message, LogLevel.Debug, user, datas, tags);
        }

        /// <summary>
        /// 警告消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="tags">标签</param>
        public void Warn(string message, params string[] tags)
        {
            Submit(message, LogLevel.Fatal, tags);
        }

        /// <summary>
        /// 警告消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="tags">标签</param>
        public void Warn(string message, ExcUserParam user, params string[] tags)
        {
            Submit(message, LogLevel.Fatal, user, tags);
        }

        /// <summary>
        /// 警告消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        public void Warn(string message, ExcDataParam data, params string[] tags)
        {
            Submit(message, LogLevel.Fatal, data, tags);
        }

        /// <summary>
        /// 警告消息
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        public void Warn(string message, List<ExcDataParam> datas, params string[] tags)
        {
            Submit(message, LogLevel.Fatal, datas, tags);
        }

        /// <summary>
        /// 警告消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        public void Warn(string message, ExcUserParam user, ExcDataParam data, params string[] tags)
        {
            Submit(message, LogLevel.Fatal, user, data, tags);
        }

        /// <summary>
        /// 警告消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        public void Warn(string message, ExcUserParam user, List<ExcDataParam> datas, params string[] tags)
        {
            Submit(message, LogLevel.Warn, user, datas, tags);
        }

        /// <summary>
        /// 致命的消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="tags">标签</param>
        public void Fatal(string message, params string[] tags)
        {
            Submit(message, LogLevel.Fatal, tags);
        }

        /// <summary>
        /// 致命的消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="tags">标签</param>
        public void Fatal(string message, ExcUserParam user, params string[] tags)
        {
            Submit(message, LogLevel.Fatal, user, tags);
        }

        /// <summary>
        /// 致命的消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        public void Fatal(string message, ExcDataParam data, params string[] tags)
        {
            Submit(message, LogLevel.Fatal, data, tags);
        }

        /// <summary>
        /// 致命的消息
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        public void Fatal(string message, List<ExcDataParam> datas, params string[] tags)
        {
            Submit(message, LogLevel.Fatal, datas, tags);
        }

        /// <summary>
        /// 致命的消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        public void Fatal(string message, ExcUserParam user, ExcDataParam data, params string[] tags)
        {
            Submit(message, LogLevel.Fatal, user, data, tags);
        }

        /// <summary>
        /// 致命的消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        public void Fatal(string message, ExcUserParam user, List<ExcDataParam> datas, params string[] tags)
        {
            Submit(message, LogLevel.Fatal, user, datas, tags);
        }

        /// <summary>
        /// 提交消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="level"></param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        private static void Submit(string message, LogLevel level, params string[] tags)
        {
            Submit(message, level, data: null, tags: tags);
        }

        /// <summary>
        /// 提交消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="level">日志等级</param>
        /// <param name="user">用户</param>
        /// <param name="tags">标签</param>
        private static void Submit(string message, LogLevel level, ExcUserParam user, params string[] tags)
        {
            Submit(message, level, user, data: null, tags: tags);
        }

        /// <summary>
        /// 提交消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="level">日志等级</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        private static void Submit(string message, LogLevel level, ExcDataParam data, params string[] tags)
        {
             
            Submit(message, level, null, data, tags);
        }

        /// <summary>
        /// 提交消息
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="level">日志等级</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        private static void Submit(string message, LogLevel level, List<ExcDataParam> datas, params string[] tags)
        {
            Submit(message, level, null, datas, tags);
        }

        /// <summary>
        /// 提交消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="level">日志等级</param>
        /// <param name="user">用户信息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        private static void Submit(string message, LogLevel level, ExcUserParam user, ExcDataParam data, params string[] tags)
        {
            var datas = new List<ExcDataParam>() { data };
            Submit(message, level, user, datas, tags);
        }

        /// <summary>
        /// 提交消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="level">日志等级</param>
        /// <param name="user">用户信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        private static void Submit(string message, LogLevel level, ExcUserParam user, List<ExcDataParam> datas, params string[] tags)
        {
            EventBuilder builder = ExceptionlessClient.Default.CreateLog(message, level)
               .AddTags(tags)
               .SetUserIdentity(user?.Id, user?.Name)
               .SetUserDescription(user?.Email, user?.Description)
               .SetReferenceId(Guid.NewGuid().ToString("N"));

            if (datas?.Count > 0)
            {
                foreach (var data in datas)
                {
                    builder.AddObject(data?.Data, data?.Name);
                }
            }
            builder.Submit();
        }
    }
}

