using NanoFabric.Exceptionless.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace NanoFabric.Exceptionless
{
    public interface ILessLinksLog
    {
        /// <summary>
        /// 提交失效链接
        /// </summary>
        /// <param name="resource">链接地址</param>
        /// <param name="tags">标签</param>
        void Submit(string resource, params string[] tags);

        /// <summary>
        /// 提交失效链接
        /// </summary>
        /// <param name="resource">链接地址</param>
        /// <param name="user">用户</param>
        /// <param name="tags">标签</param>
        void Submit(string resource, ExcUserParam user, params string[] tags);

        // <summary>
        /// 提交失效链接
        /// </summary>
        /// <param name="resource">链接地址</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        void Submit(string resource, ExcDataParam data, params string[] tags);

        /// <summary>
        /// 提交失效链接
        /// </summary>
        /// <param name="resource">链接地址</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        void Submit(string resource, List<ExcDataParam> datas, params string[] tags);

        /// <summary>
        /// 提交失效链接
        /// </summary>
        /// <param name="resource">链接地址</param>
        /// <param name="user">用户信息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        void Submit(string resource, ExcUserParam user, ExcDataParam data, params string[] tags);

        /// <summary>
        /// 提交失效链接
        /// </summary>
        /// <param name="resource">链接地址</param>
        /// <param name="user">用户信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        void Submit(string resource, ExcUserParam user, List<ExcDataParam> datas, params string[] tags);

    }
}
