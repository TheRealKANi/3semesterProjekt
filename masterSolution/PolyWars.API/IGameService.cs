using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyWars.API {
    public interface IGameService {
            event Action<IPlayer> ParticipantLoggedIn;
            event Action<string> ParticipantLoggedOut;
            event Action<string> ParticipantDisconnected;
            event Action<string> ParticipantReconnected;
            event Action ConnectionReconnecting;
            event Action ConnectionReconnected;
            event Action ConnectionClosed;
            event Action<string, string> NewTextMessage;
            HubConnection Connection { get; set; }
            Task ConnectAsync();
            Task<List<IPlayer>> LoginAsync(string name);
            Task LogoutAsync();

            Task SendBroadcastMessageAsync(string msg);
        }
    
}
