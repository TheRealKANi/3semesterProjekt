using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolyWars.Server {
    public class LobbyHub : Hub<IClient> {
        private static ConcurrentDictionary<string, User> PlayerClients = new ConcurrentDictionary<string, User>();

        public List<User> Login(string name) {
            if(!PlayerClients.ContainsKey(name)) {
                Console.WriteLine($"++ {name} logged in with id: {Context.ConnectionId}");
                List<User> users = new List<User>(PlayerClients.Values);
                User newUser = new User { Name = name, ID = Context.ConnectionId };
                var added = PlayerClients.TryAdd(name, newUser);
                if(!added) return null;
                Clients.CallerState.UserName = name;
                Clients.CallerState.ID = newUser.ID;
                Clients.Others.ParticipantLogin(newUser);
                return users;
            }
            return null;
        }
        public void Logout() {
            string name = Clients.CallerState.UserName;
            if(!string.IsNullOrEmpty(name)) {
                PlayerClients.TryRemove(name, out User client);
                Clients.Others.ParticipantLogout(name);
                Console.WriteLine($"-- {name} logged out");
            }
        }
        public override Task OnReconnected() {
            string userName = PlayerClients.SingleOrDefault((c) => c.Value.ID == Context.ConnectionId).Key;
            if(userName != null) {
                Clients.Others.ParticipantReconnection(userName);
                Console.WriteLine($"== {userName} reconnected");
            }
            return base.OnReconnected();
        }
        public override Task OnDisconnected(bool stopCalled) {
            string userName = PlayerClients.SingleOrDefault((c) => c.Value.ID == Context.ConnectionId).Key;
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
