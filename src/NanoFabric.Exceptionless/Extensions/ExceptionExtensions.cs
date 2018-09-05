using Exceptionless;
using NanoFabric.Exceptionless.Model;
using System;
using System.Collections.Generic;

namespace NanoFabric.Exceptionless.Extensions
{
    /// <summary>
    /// 基于异常的扩展
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// 提交异常
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="tags"></param>
        public static void Submit(this Exception ex, params string[] tags)
        {
            ex.Submit(data: null, tags: tags);
        }

        /// <summary>
        /// 提交异常
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="user"></param>
        /// <param name="tags"></param>
        public static void Submit(this Exception ex, ExcUserParam user, params string[] tags)
        {
            ex.Submit(user, data: null, tags: tags);
        }

        /// <summary>
        /// 提交异常
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="data"></param>
        /// <param name="level"></param>
        /// <param name="tags"></param>
        public static void Submit(this Exception ex, ExcDataParam data, params string[] tags)
        {
            ex.Submit(null, data, tags);
        }

        /// <summary>
        /// 提交异常
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        public static void Submit(this Exception ex, List<ExcDataParam> datas, params string[] tags)
        {
            ex.Submit(null, datas, tags);
        }

        /// <summary>
        /// 提交异常
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="user">用户信息</param>
        /// <param name="data"></param>
        /// <param name="tags"></param>
        public static void Submit(this Exception ex, ExcUserParam user, ExcDataParam data, params string[] tags)
        {
            var datas = new List<ExcDataParam>();
            ex.Submit(user, datas, tags);
        }

        /// <summary>
        /// 提交异常
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="user">用户信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        public static void Submit(this Exception ex, ExcUserParam user, List<ExcDataParam> datas, params string[] tags)
        {
            EventBuilder builder = ex.ToExceptionless()
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
