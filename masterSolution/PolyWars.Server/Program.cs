using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using PolyWars.API;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace PolyWars.Server {
    class Program {
        internal static bool serverLoaded = false;
        #region source https://stackoverflow.com/questions/3571627/show-hide-the-console-window-of-a-c-sharp-console-application
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        #endregion
        public static IAppBuilder app;
        private static ServiceHost host;

        internal static void Main(params string[] args) {
            string url = string.Empty;
            if(args.Length == 0 || args.Length > 0 && !args.Contains("unitTest")) {
                url = $"http://*:{Constants.serverPort}/";
            } else {
                if(args.Contains("noConsole")) {
                    IntPtr consoleHandle = GetConsoleWindow();
                    ShowWindow(consoleHandle, SW_HIDE);
                }
                if(args.Contains("unitTest")) {
                    url = $"http://127.0.0.1:{Constants.serverPort}/";
                }
            }
            // netsh http add urlacl url=http://*:5700/ user=Alle 

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
                    serverLoaded = true;
                    Console.ReadLine();
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
