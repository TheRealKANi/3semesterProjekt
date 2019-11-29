using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using PolyWars.API;

namespace PolyWars.Network {
    public class GameService : IGameService {

        public event Action<IPlayer> ParticipantLoggedIn;
        public event Action<string> ParticipantLoggedOut;
        public event Action<string> ParticipantDisconnected;
        public event Action<string> ParticipantReconnected;
        public event Action ConnectionReconnecting;
        public event Action ConnectionReconnected;
        public event Action ConnectionClosed;
        public event Action<string, string> NewTextMessage;

        private IHubProxy hubProxy;
        public HubConnection Connection { get; set; }
        private string url = "http://localhost:8080/Polywars";

        public async Task ConnectAsync() {
            Connection = new HubConnection(url);
            hubProxy = Connection.CreateHubProxy("LobbyHub");
            hubProxy.On<IPlayer>("ParticipantLogin", (u) => ParticipantLoggedIn?.Invoke(u));
            hubProxy.On<string>("ParticipantLogout", (n) => ParticipantLoggedOut?.Invoke(n));
            hubProxy.On<string>("ParticipantDisconnection", (n) => ParticipantDisconnected?.Invoke(n));
            hubProxy.On<string>("ParticipantReconnection", (n) => ParticipantReconnected?.Invoke(n));
            hubProxy.On<string, string>("BroadcastTextMessage", (author, message) => NewTextMessage?.Invoke(author, message));

            Connection.Reconnecting += Reconnecting;
            Connection.Reconnected += Reconnected;
            Connection.Closed += Disconnected;

            ServicePointManager.DefaultConnectionLimit = 10;
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

        public async Task<List<IPlayer>> LoginAsync(string name) {
            return await hubProxy.Invoke<List<IPlayer>>("Login", new object[] { name });
        }

        public async Task LogoutAsync() {
            await hubProxy.Invoke("Logout");
        }
        public async Task SendBroadcastMessageAsync(string msg) {
            await hubProxy.Invoke("BroadcastTextMessage", msg);
        }
        

    }
}
