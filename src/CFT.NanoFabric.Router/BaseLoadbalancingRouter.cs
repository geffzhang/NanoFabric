using System;
using System.Collections.Generic;
using System.Text;

namespace CFT.NanoFabric.Router
{
    /// <summary>
    /// 负载均衡基础路由
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseLoadbalancingRouter<T> : IRouter<T>
        where T : class
    {
        protected T Previous;

        /// <summary>
        /// 选择下一个处理的对象
        /// </summary>
        /// <param name="instances"></param>
        /// <returns></returns>
        public T Choose(IList<T> instances)
        {
            var next = Choose(Previous, instances);
            Previous = next;
            return next;
        }

        /// <summary>
        /// 负载均衡处理的具体实现算法在子类中实现
        /// </summary>
        /// <param name="previous">上次处理的对象</param>
        /// <param name="instances">实例列表</param>
        /// <returns></returns>
        public  abstract T  Choose(T previous, IList<T> instances);
       
    }
}
