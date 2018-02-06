using System;
using System.Collections.Generic;
using System.Linq;
//using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using GaiaProject.Models;
using Gaia.Service;
using ManageTool;
using GaiaProject.Data;
using GaiaDbContext.Models;
using GaiaProject.Notice;
using Microsoft.AspNetCore.Http;
using UEditorNetCore;


namespace GaiaProject
{
    public class Startup
    {
        /// <summary>
        /// 是否启用好友系统
        /// </summary>
        public static bool isEnableFriend = true;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();
            }
            else
            {
                //启动备份计划
                DaemonMgr.StartAll();
                GaiaCore.Gaia.GameMgr.RestoreDictionary(string.Empty);
            }
            ServerStatus.ServerStartTime = DateTime.Now;
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("GaiaProject")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            //services.AddMvc();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.Configure<IdentityOptions>(options =>
            {
                //邮箱不允许重复
                options.User.RequireUniqueEmail = true;
                //允许使用用户名
                //options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(3650);
            });

            //第一个参数为配置文件路径，默认为项目目录下config.json
            //第二个参数为是否缓存配置文件，默认false
            services.AddUEditorService();
            //缓存系统
            services.AddMemoryCache();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
                Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration.Active.DisableTelemetry = true;
                Microsoft.ApplicationInsights.Extensibility.Implementation.TelemetryDebugWriter.IsTracingDisabled = true;                    
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseIdentity();


            //websocket中间件，需要在mvc之前声明
            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 4 * 1024
            };
            app.UseWebSockets(webSocketOptions);
            app.UseMiddleware<NoticeWebSocketMiddleware>();

            // Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });



//            // Configure function 
//            // 这里主要是监听 WebSocket的请求，然后Invoke Echo 方法进行相关操作。比如，它接受到浏览器发来 WebSocket 的Close 命令了，那么在Echo 方法直接 await webSocket.CloseAsync(result.CloseStatus.Value... 相关操作
//            app.Use(async (context, next) =>
//            {
//                if (context.Request.Path == "/ws")
//                {
//                    if (context.WebSockets.IsWebSocketRequest)
//                    {
//                        WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
//                        await Echo(context, webSocket);
//                    }
//                    else
//                    {
//                        context.Response.StatusCode = 400;
//                    }
//                }
//            });

        }



    }
}
