using Microsoft.AspNet.SignalR.Client;
using PolyWars.API;
using PolyWars.API.Network;
using System;
using System.Net;
using System.Threading.Tasks;

namespace PolyWars.Network {
    public class GameService : IGameService {

        public event Action<string> announceClientLoggedIn;
        public event Action<string> ClientLoggedOut;
        public event Action<string> ClientDisconnected;
        public event Action<string> ClientReconnected;
        public event Action<string> AccessDenied;
        public event Action ConnectionReconnecting;
        public event Action ConnectionReconnected;
        public event Action ConnectionClosed;
        public event Action<string, string> NewTextMessage;
        //public event Action<string> OnConnected;

        private IHubProxy hubProxy;
        public HubConnection Connection { get; set; }
        //private string url = "http://polywars.servegame.com:8080/Polywars";
        private string url = "http://localhost:8080/Polywars";

        public async Task ConnectAsync() {
            Connection = new HubConnection(url);
            hubProxy = Connection.CreateHubProxy("MainHub");
            hubProxy.On<string>("announceClientLoggedIn", (u) => announceClientLoggedIn?.Invoke(u));
            hubProxy.On<string>("ClientLogout", (n) => ClientLoggedOut?.Invoke(n));
            hubProxy.On<string>("ClientDisconnected", (n) => ClientDisconnected?.Invoke(n));
            hubProxy.On<string>("ClientReconnected", (n) => ClientReconnected?.Invoke(n));
            //hubProxy.On<string>("OnConnected", (n) => OnConnected?.Invoke(n));
            hubProxy.On<string>("AccessDenied", (n) => AccessDenied?.Invoke(n));

            Connection.Reconnecting += Reconnecting;
            Connection.Reconnected += Reconnected;
            Connection.Closed += Disconnected;

            ServicePointManager.DefaultConnectionLimit = 100;
            await Connection.Start();
        }

        public async Task<bool> PlayerMovedAsync(IRay playerIRay) {
            return await hubProxy.Invoke<bool>("PlayerMoved", playerIRay);
        }

        public async Task<IUser> LoginAsync(string name, string hashedPassword) {
            return await hubProxy.Invoke<User>("Login", name, hashedPassword);
            
        }

        public async Task LogoutAsync() {
            await hubProxy.Invoke("Logout");
        }

        public async Task SendBroadcastMessageAsync(string msg) {
            await hubProxy.Invoke("BroadcastTextMessage", msg);
        }


        private void Disconnected() { ConnectionClosed?.Invoke(); }

        private void Reconnected() { ConnectionReconnected?.Invoke(); }

        private void Reconnecting() { ConnectionReconnecting?.Invoke(); }
    }
}
