using NanoFabric.Exceptionless.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace NanoFabric.Exceptionless
{
    public interface ILessLog
    {
        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="tags">标签</param>
        void Info(string message, params string[] tags);

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="tags">标签</param>
        void Info(string message, ExcUserParam user, params string[] tags);
 
        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        void Info(string message, ExcDataParam data, params string[] tags); 

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        void Info(string message, List<ExcDataParam> datas, params string[] tags);

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        void Info(string message, ExcUserParam user, ExcDataParam data, params string[] tags);

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        void Info(string message, ExcUserParam user, List<ExcDataParam> datas, params string[] tags);   

        /// <summary>
        /// 异常消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="tags">标签</param>
        void Error(string message, params string[] tags);
 

        /// <summary>
        /// 异常消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="tags">标签</param>
        void Error(string message, ExcUserParam user, params string[] tags);


        /// <summary>
        /// 异常消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        void Error(string message, ExcDataParam data, params string[] tags);


        /// <summary>
        /// 异常消息
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        void Error(string message, List<ExcDataParam> datas, params string[] tags);


        /// <summary>
        /// 异常消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        void Error(string message, ExcUserParam user, ExcDataParam data, params string[] tags);
  

        /// <summary>
        /// 异常消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        void Error(string message, ExcUserParam user, List<ExcDataParam> datas, params string[] tags);  

        /// <summary>
        /// 跟踪信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="tags">标签</param>
        void Trace(string message, params string[] tags);
  

        /// <summary>
        /// 跟踪信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="tags">标签</param>
        void Trace(string message, ExcUserParam user, params string[] tags);


        /// <summary>
        /// 跟踪信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        void Trace(string message, ExcDataParam data, params string[] tags);


        /// <summary>
        /// 跟踪信息
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        void Trace(string message, List<ExcDataParam> datas, params string[] tags);

        /// <summary>
        /// 跟踪信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        void Trace(string message, ExcUserParam user, ExcDataParam data, params string[] tags);

        /// <summary>
        /// 跟踪信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        void Trace(string message, ExcUserParam user, List<ExcDataParam> datas, params string[] tags);


        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="tags">标签</param>
        void Debug(string message, params string[] tags);

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="tags">标签</param>
        void Debug(string message, ExcUserParam user, params string[] tags);


        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        void Debug(string message, ExcDataParam data, params string[] tags);


        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        void Debug(string message, List<ExcDataParam> datas, params string[] tags);


        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        void Debug(string message, ExcUserParam user, ExcDataParam data, params string[] tags);


        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        void Debug(string message, ExcUserParam user, List<ExcDataParam> datas, params string[] tags);
     

        /// <summary>
        /// 警告消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="tags">标签</param>
        void Warn(string message, params string[] tags);

        /// <summary>
        /// 警告消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="tags">标签</param>
        void Warn(string message, ExcUserParam user, params string[] tags);


        /// <summary>
        /// 警告消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        void Warn(string message, ExcDataParam data, params string[] tags);

        /// <summary>
        /// 警告消息
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        void Warn(string message, List<ExcDataParam> datas, params string[] tags);
  

        /// <summary>
        /// 警告消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        void Warn(string message, ExcUserParam user, ExcDataParam data, params string[] tags);


        /// <summary>
        /// 警告消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        void Warn(string message, ExcUserParam user, List<ExcDataParam> datas, params string[] tags);

        /// <summary>
        /// 致命的消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="tags">标签</param>
        void Fatal(string message, params string[] tags);

        /// <summary>
        /// 致命的消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="tags">标签</param>
        void Fatal(string message, ExcUserParam user, params string[] tags);


        /// <summary>
        /// 致命的消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        void Fatal(string message, ExcDataParam data, params string[] tags);


        /// <summary>
        /// 致命的消息
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        void Fatal(string message, List<ExcDataParam> datas, params string[] tags);

        /// <summary>
        /// 致命的消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="data">自定义数据</param>
        /// <param name="tags">标签</param>
        void Fatal(string message, ExcUserParam user, ExcDataParam data, params string[] tags);
 

        /// <summary>
        /// 致命的消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="user">用户信息</param>
        /// <param name="datas">自定义数据</param>
        /// <param name="tags">标签</param>
        void Fatal(string message, ExcUserParam user, List<ExcDataParam> datas, params string[] tags);
        
    }
}
