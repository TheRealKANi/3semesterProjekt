using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.Cors;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyWars.Server
{
    namespace ChatServerCS {
        class Program {
            static void Main( string[] args ) {
                string url = "http://localhost:8080/";
                using( WebApp.Start<Startup>( url ) ) {
                    Console.WriteLine( $"Server running at {url}" );
                    Console.ReadLine();
                }
            }
        }

        public class Startup {
            public void Configuration( IAppBuilder app ) {
                app.UseCors( CorsOptions.AllowAll );
                app.MapSignalR( "/polyWars", new HubConfiguration() );

                GlobalHost.Configuration.MaxIncomingWebSocketMessageSize = null;
            }
        }
    }
}
