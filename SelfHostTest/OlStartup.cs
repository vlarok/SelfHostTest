using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Owin;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;

using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;

using Microsoft.Owin.Cors;

namespace SelfHostTest
{
    public class OlStartup
    {
        // This code configures Web API. The OlStartup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {/*
            string str = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),"webdir");
            appBuilder.UseStaticFiles(str);
            */
            var root = AppDomain.CurrentDomain.BaseDirectory;
            var fileServerOptions = new FileServerOptions()
            {
                EnableDefaultFiles = true,
                EnableDirectoryBrowsing = true,
                FileSystem = new PhysicalFileSystem(@"C:\Partcom\VoiceloggerSip\Client")
            };
            appBuilder.UseFileServer(fileServerOptions);


            appBuilder.UseCors(CorsOptions.AllowAll);
            appBuilder.MapSignalR();
            /*
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            appBuilder.UseWebApi(config);
            */
        }
    }

    public class MyHub : Hub
    {
        public void Send(Envelope env)
        {
           
            Clients.All.addMessage(env);
        }

        public void Posting(Envelope env)
        {
            
            Clients.All.addMessage(env);
        }
    }
}
