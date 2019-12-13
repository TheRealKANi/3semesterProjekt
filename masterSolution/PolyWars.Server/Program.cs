using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using PolyWars.API;
using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace PolyWars.Server {
    namespace ChatServerCS {
        class Program {
            public static IAppBuilder app;
            private static ServiceHost host;

            static void Main(string[] args) {

                string url = $"http://*:{Constants.serverPort}/";
                // netsh http add urlacl url=http://*:5700/ user=Alle // only if not started as an admin user 

                // ServiceBinding 
                string serviceUrl = $"{Constants.protocol}localhost:{Constants.servicePort}{Constants.serviceEndPoint}";
                // netsh http add urlacl url=http://+:5701/ user=Alle 
                Console.WriteLine(serviceUrl);
                Uri baseAddress = new Uri(serviceUrl);

                host = new ServiceHost(typeof(Services.WebClientService), baseAddress);
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;

                host.Description.Behaviors.Add(smb);
                // Since no endpoints are explicitly configured, the runtime will create 
                // one endpoint per base address for each service contract implemented 
                // by the service. 
                host.Open();
                while(true) {
                    using(WebApp.Start<Startup>(url)) {
                        Console.WriteLine($"Server running at {url}");

                        Console.ReadLine();
                    }
                }
            }
        }

        public class Startup {
            public void Configuration(IAppBuilder app) {
                Program.app = app;
                app.UseCors(CorsOptions.AllowAll);
                app.MapSignalR(Constants.serverEndPoint, new HubConfiguration() { EnableDetailedErrors = true });



                GlobalHost.Configuration.MaxIncomingWebSocketMessageSize = null;
            }
        }
    }
}
