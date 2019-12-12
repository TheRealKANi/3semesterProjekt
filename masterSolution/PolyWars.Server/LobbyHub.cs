using Microsoft.AspNet.SignalR;
using PolyWars.API;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolyWars.Server {
    public class LobbyHub : Hub<IClient> {

        private static ConcurrentDictionary<string, IUser> LobbyClients = new ConcurrentDictionary<string, IUser>();
        private static ConcurrentDictionary<string, ILobby> Lobbies = new ConcurrentDictionary<string, ILobby>();

        public LobbyHub() {
            foreach(Lobby lobby in LobbyController.listLobbies()) {
                Lobbies.TryAdd(lobby.Name, lobby);
            }
            LobbyClients.TryAdd("silas", new User() {
                Name = "silas",
                ID = "1",
                HashedPassword = "123"
            });

        }

        public string Login(IUser user) {
            // Player logs in via username and password 
            // server either verifyes or denies
            // when verifyed, server sends list of lobbies for player to join
            if(LobbyClients.ContainsKey(user.Name)) {
                if(LobbyClients[user.Name].HashedPassword.Equals(user.HashedPassword)) {
                    Console.WriteLine($"++ {user.Name} logged into lobby with id: {Context.ConnectionId}");

                    // Creates new list from old list values
                    List<IUser> users = new List<IUser>(LobbyClients.Values);

                    // Creates new user based on the name and ID of connection

                    // Tries to add new user to list of lobby clients
                    var added = LobbyClients.TryAdd(user.Name, user);
                    if(!added) return null;

                    // Sends the username back to client as a 'ok'
                    Clients.CallerState.UserName = user.Name;
                    Clients.CallerState.DefaultLobby = LobbyController.DefaultLobby;

                    // Notifyes other lobby clients that a new user has joined the lobby
                    //Clients.Others.ParticipantLogin(newUser);

                    // Generate list of lobbies to send back to client
                    List<ILobby> lobbies = new List<ILobby>(Lobbies.Values);
                    return lobbies;
                } else {
                    Clients.Caller.DeniedLogin("Wrong username or password");
                }

            }
            return null;
        }

        public List<ILobby> GetLobbies() {
            List<ILobby> lobbies = new List<ILobby>(Lobbies.Values);
            return lobbies;
        }

        public void Logout() {
            string name = Clients.CallerState.UserName;
            if(!string.IsNullOrEmpty(name)) {
                LobbyClients.TryRemove(name, out User client);
                Clients.Others.ParticipantLogout(name);
                Console.WriteLine($"-- {name} logged out");
            }
        }

        public override Task OnReconnected() {
            string userName = LobbyClients.SingleOrDefault((c) => c.Value.ID == Context.ConnectionId).Key;
            if(userName != null) {
                Clients.Others.ParticipantReconnection(userName);
                Console.WriteLine($"== {userName} reconnected");
            }
            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled) {
            string userName = LobbyClients.SingleOrDefault((c) => c.Value.ID == Context.ConnectionId).Key;
            if(userName != null) {
                Clients.Others.ParticipantDisconnection(userName);
                Console.WriteLine($"<> {userName} disconnected");
            }
            return base.OnDisconnected(stopCalled);
        }

        public void BroadcastTextMessage(string message) {
            string name = Clients.CallerState.UserName;
            if(!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(message)) {
                Clients.Others.BroadcastTextMessage(name, message);
            }
        }
    }
}
