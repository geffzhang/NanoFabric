using Exceptionless;
using NanoFabric.Exceptionless.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace NanoFabric.Exceptionless
{
    public class LessFeatureLog : ILessFeatureLog
    {
        /// <summary>
        /// 提交特性
        /// </summary>
        /// <param name="feature">特性信息</param>
        /// <param name="tags">标签</param>
        public void Submit(string feature, params string[] tags)
        {
            Submit(feature, data: null, tags: tags);
        }

        /// <summary>
        /// 提交特性
        /// </summary>
        /// <param name="feature">消息</param>
        /// <param name="user">用户</param>
        /// <param name="tags">标签</param>
        public void Submit(string feature, ExcUserParam user, params string[] tags)
        {
            Submit(feature, user, data: null, tags: tags);
        }

        /// <summary>
        /// 提交特性
        /// </summary>
        /// <param name="feature">特性信息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        public void Submit(string feature, ExcDataParam data, params string[] tags)
        {
            Submit(feature, null, data, tags);
        }

        /// <summary>
        /// 提交特性
        /// </summary>
        /// <param name="feature">特性信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        public void Submit(string feature, List<ExcDataParam> datas, params string[] tags)
        {
            Submit(feature, null, datas, tags);
        }

        /// <summary>
        /// 提交特性
        /// </summary>
        /// <param name="feature">特性消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        public void Submit(string feature, ExcUserParam user, ExcDataParam data, params string[] tags)
        {
            var datas = new List<ExcDataParam>() { data };
            Submit(feature, user, datas, tags);
        }

        /// <summary>
        /// 提交特性
        /// </summary>
        /// <param name="feature">特性信息</param>
        /// <param name="user">用户信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        public void Submit(string feature, ExcUserParam user, List<ExcDataParam> datas, params string[] tags)
        {
            EventBuilder builder = ExceptionlessClient.Default.CreateFeatureUsage(feature)
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
