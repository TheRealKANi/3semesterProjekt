using Microsoft.AspNet.SignalR;
using PolyWars.API.Network;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace PolyWars.Server.Hubs {
    class MainHub : Hub<IClient> {
        private static ConcurrentDictionary<string, IUser> PlayerClients = new ConcurrentDictionary<string, IUser>();


        // TODO should get user from database
        public IUser Login(string name) {
            if(!PlayerClients.ContainsKey(name)) {
                Console.WriteLine($"++ {name} logged in with id: {Context.ConnectionId}");
                IUser newUser = new User { Name = name, ID = Context.ConnectionId };
                bool added = PlayerClients.TryAdd(name, newUser);

                if(!added) {
                    return null;
                }

                Clients.CallerState.UserName = name;
                Clients.CallerState.ID = newUser.ID;
                Clients.Others.ClientLogin(newUser);
                return newUser;
            }
            return null;
        }
        public void Logout() {
            string name = Clients.CallerState.UserName;
            if(!string.IsNullOrEmpty(name)) {
                PlayerClients.TryRemove(name, out IUser client);
                Clients.Others.ClientLogout(name);
                Console.WriteLine($"-- {name} logged out");
            }
        }
        public override Task OnReconnected() {
            string userName = PlayerClients.SingleOrDefault((c) => c.Value.ID == Context.ConnectionId).Key;
            if(userName != null) {
                Clients.Others.ClientReconnected(userName);
                Console.WriteLine($"== {userName} reconnected");
            }
            return base.OnReconnected();
        }
        public override Task OnDisconnected(bool stopCalled) {
            string userName = PlayerClients.SingleOrDefault((c) => c.Value.ID == Context.ConnectionId).Key;
            if(userName != null) {
                Clients.Others.ClientDisconnected(userName);
                Console.WriteLine($"<> {userName} disconnected");
            }
            return base.OnDisconnected(stopCalled);
        }
    }
}
