using Exceptionless;
using NanoFabric.Exceptionless.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace NanoFabric.Exceptionless
{
    /// <summary>
    /// 失效链接
    /// </summary>
    public class LessLinksLog : ILessLinksLog
    {
        /// <summary>
        /// 提交失效链接
        /// </summary>
        /// <param name="resource">链接地址</param>
        /// <param name="tags">标签</param>
        public void Submit(string resource, params string[] tags)
        {
            Submit(resource, data: null, tags: tags);
        }

        /// <summary>
        /// 提交失效链接
        /// </summary>
        /// <param name="resource">链接地址</param>
        /// <param name="user">用户</param>
        /// <param name="tags">标签</param>
        public void Submit(string resource, ExcUserParam user, params string[] tags)
        {
            Submit(resource, user, data: null, tags: tags);
        }

        /// <summary>
        /// 提交失效链接
        /// </summary>
        /// <param name="resource">链接地址</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        public void Submit(string resource, ExcDataParam data, params string[] tags)
        {
            Submit(resource, null, data, tags);
        }

        /// <summary>
        /// 提交失效链接
        /// </summary>
        /// <param name="resource">链接地址</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        public void Submit(string resource, List<ExcDataParam> datas, params string[] tags)
        {
            Submit(resource, null, datas, tags);
        }

        /// <summary>
        /// 提交失效链接
        /// </summary>
        /// <param name="resource">链接地址</param>
        /// <param name="user">用户信息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        public void Submit(string resource, ExcUserParam user, ExcDataParam data, params string[] tags)
        {
            var datas = new List<ExcDataParam>() { data };
            Submit(resource, user, datas, tags);
        }

        /// <summary>
        /// 提交失效链接
        /// </summary>
        /// <param name="resource">链接地址</param>
        /// <param name="user">用户信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        public void Submit(string resource, ExcUserParam user, List<ExcDataParam> datas, params string[] tags)
        {
            EventBuilder builder = ExceptionlessClient.Default.CreateNotFound(resource)
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
