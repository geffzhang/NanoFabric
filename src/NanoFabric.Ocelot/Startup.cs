using Exceptionless;
using Exceptionless.Models;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NanoFabric.AppMetrics;
using NanoFabric.AspNetCore;
using NanoFabric.Exceptionless;
using NanoFabric.Exceptionless.Model;
using NLog.Extensions.Logging;
using NLog.Web;
using Ocelot.Administration;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Polly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NanoFabric.Ocelot
{
    public class Startup
    {
        /// <summary>
        /// 构造函数，初始化配置信息
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 系统配置
        /// </summary>
        public IConfiguration Configuration { get; }

        /// This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var authority = Configuration.GetValue<string>("Authority");
            var collectorUrl = Configuration.GetValue<string>("Skywalking:CollectorUrl");

            Action<IdentityServerAuthenticationOptions> options = o =>
            {
                o.Authority = authority;
                o.ApiName = "api";
                o.SupportedTokens = SupportedTokens.Both;
                o.ApiSecret = "secret";
            };

            var authenticationProviderKey = "apikey";
            Action<IdentityServerAuthenticationOptions> options2 = o =>
            {
                o.Authority = "http://127.0.0.1:5000";
                o.ApiName = "api1";
                o.RequireHttpsMetadata = false;
            };

            services.AddAuthentication()
            .AddIdentityServerAuthentication(authenticationProviderKey, options2);

            services.AddOcelot()
                .AddCacheManager(x =>
                {
                    x.WithDictionaryHandle();
                })
                 .AddConsul()
                 .AddConfigStoredInConsul()
                 .AddPolly()
                 .AddAdministration("/administration", options);

            services.AddNanoFabricConsul(Configuration);
            services.AddNanoFabricExceptionless();

            services.AddAppMetrics(x =>
            {
                var opt = Configuration.GetSection("AppMetrics").Get<AppMetricsOptions>();
                x.App = opt.App;
                x.ConnectionString = opt.ConnectionString;
                x.DataBaseName = opt.DataBaseName;
                x.Env = opt.Env;
                x.Password = opt.Password;
                x.UserName = opt.UserName;
            });

            //services.AddSkyWalking(option =>
            //{
            //    option.DirectServers = collectorUrl;
            //    option.ApplicationCode = "nanofabric_ocelot";
            //});
        }

        // http://edi.wang/post/2017/11/1/use-nlog-aspnet-20
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime applicationLifetime)
        {
            app.UseExceptionless(Configuration);
            env.ConfigureNLog($"{env.ContentRootPath}{ Path.DirectorySeparatorChar}nlog.config");
            //add NLog to ASP.NET Core
            loggerFactory.AddNLog();
            loggerFactory.AddExceptionless();

            app.UseConsulRegisterService(Configuration);

            app.UseOcelot().Wait();
            app.UseAppMetrics();
            ExceptionlessClient.Default.SubmittingEvent += Default_SubmittingEvent;

            string tagName = "消息标签";//自定义标签
            var data = new ExcDataParam() { Name = "请求参数", Data = new { Id = 001, Name = "张三" } };//自定义单个model
            var user = new ExcUserParam() { Id = "No0001", Name = "张善友", Email = "geffzhang@live.cn", Description = "高级开发工程师" };//用户信息
            var datas = new List<ExcDataParam>() {
                new ExcDataParam(){
                    Name ="请求参数",
                    Data =new { Id = 002, Name = "李四"
                    }
                },
                new ExcDataParam(){
                    Name ="返回结果",
                    Data =new { Id = 003, Name = "王五"
                    }
                }
            };

            ILessLog lessLog = app.ApplicationServices.GetRequiredService<ILessLog>();
            lessLog.Info("带用户&自定义数据&标签: Application - Configure is invoked", user, datas, tagName);
        }

        /// <summary>
        /// 默认提交异常处理事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Default_SubmittingEvent(object sender, EventSubmittingEventArgs e)
        {
            var argEvent = e.Event;
            if (argEvent.Type == Event.KnownTypes.Log && argEvent.Source == "Ocelot.Configuration.Repository.FileConfigurationPoller")
            {
                e.Cancel = true;
                return;
            }
            // 只处理未处理的异常
            if (!e.IsUnhandledError)
                return;

            //忽略没有错误体的错误
            var error = argEvent.GetError();
            if (error == null)
                return;

            // 忽略404错误
            if (e.Event.IsNotFound())
            {
                e.Cancel = true;
                return;
            }

            //忽略401(Unauthorized)和请求验证的错误.
            if (error.Code == "401")
            {
                e.Cancel = true;
                return;
            }

            //忽略任何未被代码抛出的异常
            var handledNamespaces = new List<string> { "Exceptionless" };
            var handledNamespaceList = error.StackTrace.Select(s => s.DeclaringNamespace).Distinct();
            if (!handledNamespaceList.Any(ns => handledNamespaces.Any(ns.Contains)))
            {
                e.Cancel = true;
                return;
            }

            e.Event.Tags.Add("未捕获异常");//添加系统异常标签
            e.Event.MarkAsCritical();//标记为关键异常
        }
    }
}