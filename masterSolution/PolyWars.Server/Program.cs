using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.Cors;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;

namespace PolyWars.Server
{
    namespace ChatServerCS {
        class Program {
            public static IAppBuilder app;
            static void Main( string[] args ) {
                string url = "http://192.168.0.46:8080";

                while(true) {
                    using(WebApp.Start<Startup>(url)) {
                        Console.WriteLine( $"Server running at {url}" );

                        Console.ReadLine();
                    }
                }
            }
        }

        public class Startup {
            public void Configuration( IAppBuilder app ) {
                Program.app = app;
                app.UseCors( CorsOptions.AllowAll );
                app.MapSignalR("/Polywars", new HubConfiguration() { EnableDetailedErrors = true});
                
                GlobalHost.Configuration.MaxIncomingWebSocketMessageSize = null;
            }
        }
    }
}
