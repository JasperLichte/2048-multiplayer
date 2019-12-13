using System;
using System.IO;
using System.Net.WebSockets;
using System.Threading;
using log4net.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using server.model;
using server.websocket;
using log4net;
using System.Reflection;

namespace server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials();
            }));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseBrowserLink();
            }
            else
            {
                app.UseHsts();

            }
            app.UseDefaultFiles();
            app.UseStaticFiles();
            WebSocketOptions webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(60),
                ReceiveBufferSize = 8192
            };
            //enable websocket with prior declared options
            app.UseWebSockets(webSocketOptions);
            app.UseMvc();
            //use self defined policy(see Configure Services) for Cross-Origin Resource Sharing (CORS)
            app.UseCors("MyPolicy");
            Config config = Config.loadConfig();
            ThreadPool.SetMinThreads(config.maxUsers, config.maxUsers);
            app.Use(async (context, next) =>
            {
                //catch http requests at /server and check if it is a websocket request
                if (context.Request.Path == "/server")
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                        //every websocket has its own SocketHandler
                        SocketHandler doorSocket = new SocketHandler();
                        await doorSocket.socketHandle(context, webSocket);
                    }
                    else
                    {
                        await next();
                    }
                }
            });

        }
    }
}
