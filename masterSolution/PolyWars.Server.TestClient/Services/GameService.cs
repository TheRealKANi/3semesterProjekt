using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace PolyWars.Server.TestClient.Services {
    public class GameService {

        public event Action<string> ParticipantLoggedIn;
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
            hubProxy.On<string>("ParticipantLogin", (username) => ParticipantLoggedIn?.Invoke(username));
            hubProxy.On<string>("ParticipantLogout", (username) => ParticipantLoggedOut?.Invoke(username));
            hubProxy.On<string>("ParticipantDisconnection", (username) => ParticipantDisconnected?.Invoke(username));
            hubProxy.On<string>("ParticipantReconnection", (username) => ParticipantReconnected?.Invoke(username));
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

        public async Task<List<ILobby>> LoginAsync(IUser user) {
            return await hubProxy.Invoke<List<ILobby>>("Login", new object[] { user.Name, user.HashedPassword });
        }

        public async Task<List<ILobby>> GetLobbies() {
            return await hubProxy.Invoke<List<ILobby>>("GetLobbies");
        }

        public async Task LogoutAsync() {
            await hubProxy.Invoke("Logout");
        }
        public async Task SendBroadcastMessageAsync(string msg) {
            await hubProxy.Invoke("BroadcastTextMessage", msg);
        }

        public void DeniedLogin(string msg) {
            Console.WriteLine(msg);
        }

    }
}
