using Microsoft.AspNet.SignalR;
using PolyWars.API;
using PolyWars.API.Network;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolyWars.Server.Hubs {
    public class LobbyHub : Hub<IClient> {
        private static ConcurrentDictionary<string, User> PlayerClients = new ConcurrentDictionary<string, User>();

        //public IUser Login(string name) {
        //    if(!PlayerClients.ContainsKey(name)) {
        //        Console.WriteLine($"++ {name} logged in with id: {Context.ConnectionId}");
        //        User newUser = new User { Name = name, ID = Context.ConnectionId };
        //        var added = PlayerClients.TryAdd(name, newUser);
        //        if(!added) return null;
        //        Clients.Others.announceClientLoggedIn(newUser.Name);
        //        return newUser;
        //    }
        //    return null;
        //}
        public void Logout() {
            string name = Clients.CallerState.UserName;
            if(!string.IsNullOrEmpty(name)) {
                PlayerClients.TryRemove(name, out User client);
                Clients.Others.clientLogout(name);
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
        public void BroadcastTextMessage(string message) {
            string name = Clients.CallerState.UserName;
            if(!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(message)) {
                Clients.Others.BroadcastTextMessage(name, message);
            }
        }
    }
}
