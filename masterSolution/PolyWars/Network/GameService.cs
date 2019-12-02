using Microsoft.AspNet.SignalR.Client;
using PolyWars.API.Network;
using System;
using System.Net;
using System.Threading.Tasks;

namespace PolyWars.Network {
    public class GameService : IGameService {

        public event Action<IUser> ClientLoggedIn;
        public event Action<string> ClientLoggedOut;
        public event Action<string> ClientDisconnected;
        public event Action<string> ClientReconnected;
        public event Action ConnectionReconnecting;
        public event Action ConnectionReconnected;
        public event Action ConnectionClosed;
        public event Action<string, string> NewTextMessage;

        private IHubProxy hubProxy;
        public HubConnection Connection { get; set; }
        private string url = "http://192.168.0.46:8080/Polywars";

        public async Task ConnectAsync() {
            Connection = new HubConnection(url);
            hubProxy = Connection.CreateHubProxy("ServerHub");
            hubProxy.On<IUser>("ClientLogin", (u) => ClientLoggedIn?.Invoke(u));
            hubProxy.On<string>("ClientLogout", (n) => ClientLoggedOut?.Invoke(n));
            hubProxy.On<string>("ClientDisconnected", (n) => ClientDisconnected?.Invoke(n));
            hubProxy.On<string>("ClientReconnected", (n) => ClientReconnected?.Invoke(n));

            Connection.Reconnecting += Reconnecting;
            Connection.Reconnected += Reconnected;
            Connection.Closed += Disconnected;

            ServicePointManager.DefaultConnectionLimit = 100;
            await Connection.Start();
        }
        private void Disconnected() {
            ConnectionClosed?.Invoke();
        }

        private void Reconnected() {
            ConnectionReconnected?.Invoke();
        }

        private void Reconnecting() {
            ConnectionReconnecting?.Invoke();
        }

        public async Task<IUser> LoginAsync(string name) {
            return await hubProxy.Invoke<IUser>("Login", new object[] { name });
        }

        public async Task LogoutAsync() {
            await hubProxy.Invoke("Logout");
        }
        public async Task SendBroadcastMessageAsync(string msg) {
            await hubProxy.Invoke("BroadcastTextMessage", msg);
        }


    }
}
