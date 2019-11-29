using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace PolyWars.Server {
    class LobbyHub : Hub<IClient> {
        private static ConcurrentDictionary<string, User> PlayerClients = new ConcurrentDictionary<string, User>();

        public bool Login(string name) {
            bool addSucceeded = false;
            if(!PlayerClients.ContainsKey(name)) {
                User user = new User(name, Context.ConnectionId);

                addSucceeded = PlayerClients.TryAdd(name, user);

                Clients.CallerState.UserName = name;
            }
            return addSucceeded;
        }
        public void Logout() {
            string name = Clients.CallerState.UserName;
            if(!string.IsNullOrEmpty(name)) {
                User client = new User();
                PlayerClients.TryRemove(name, out client);
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
