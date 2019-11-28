using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyWars.Server {
    class Lobby : Hub<IClient> {
        private static ConcurrentDictionary<string, User> PlayerClients = new ConcurrentDictionary<string, User>();

        public bool Login(string name ) {
            bool addSucceeded;
            if( !PlayerClients.ContainsKey( name ) ) {
                User user = new User( name, Context.ConnectionId );
                
                addSucceeded = PlayerClients.TryAdd( name, user );

                Clients.CallerState.UserName = name;
            }
            return addSucceeded;
        }
    }
}
