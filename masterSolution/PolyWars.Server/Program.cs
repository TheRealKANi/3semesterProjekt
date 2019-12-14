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
using System.Threading;

namespace PolyWars.Server {
    class Program {
        internal static bool serverLoaded = false;
        private static bool isUnitTesting = false;
        private static bool showConsole = true;
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
        internal static bool shutdown;
        internal static void Main(params string[] args) {
            string url = string.Empty;
            shutdown = false;
            if(args.Length == 0 || args.Length > 0 && !args.Contains("unitTest")) {
                url = $"http://*:{Constants.serverPort}/";
            } else {
                if(args.Contains("noConsole")) {
                    showConsole = false;
                    
                }
                if(args.Contains("unitTest")) {
                    isUnitTesting = true;
                    showConsole = false;
                }
            }
            if(!showConsole) {
                IntPtr consoleHandle = GetConsoleWindow();
                ShowWindow(consoleHandle, SW_HIDE);
            }
            if(isUnitTesting) {
                url = $"http://127.0.0.1:{Constants.serverPort}/";
            }
            string serviceUrl = $"{Constants.protocol}localhost:{Constants.servicePort}{Constants.serviceEndPoint}";

            Thread gameServiceThread = new Thread(() => OpenGameServer(url)) { IsBackground = true };
            Thread webServiceThread = new Thread(() => OpenWebService(serviceUrl)) { IsBackground = true };

            gameServiceThread.Start();
            webServiceThread.Start();

            if(showConsole) {
                Console.WriteLine("Press the anykey to shut down server");
                Console.ReadLine();
                shutdownServer();
            }
        }

        public static void shutdownServer() {
            shutdown = true;
        }

        private static void OpenWebService(string serviceUrl) {

            // ServiceBinding 
            // netsh http add urlacl url=http://+:5701/ user=Alle 
            Console.WriteLine(serviceUrl);
            Uri baseAddress = new Uri(serviceUrl);

            host = new ServiceHost(typeof(Services.WebClientService), baseAddress);
            ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
            smb.HttpGetEnabled = true;
            smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
            host.Description.Behaviors.Add(smb);

            // Service Debug Info
            ServiceDebugBehavior debug = host.Description.Behaviors.Find<ServiceDebugBehavior>();
            if(debug == null) {
                host.Description.Behaviors.Add(
                     new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });
            } else {
                // make sure setting is turned ON
                if(!debug.IncludeExceptionDetailInFaults) {
                    debug.IncludeExceptionDetailInFaults = true;
                }
            }


            // Since no endpoints are explicitly configured, the runtime will create 
            // one endpoint per base address for each service contract implemented 
            // by the service. 
            host.Open();

            while(!shutdown) { Thread.Sleep(1000); }
            host.Close();
        }

        private static void OpenGameServer(string url) {
            // netsh http add urlacl url=http://*:5700/ user=Alle 
            using(WebApp.Start<Startup>(url)) {
                Console.WriteLine($"Server running at {url}");
                serverLoaded = true;
                while(!shutdown) { Thread.Sleep(5000); }
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
