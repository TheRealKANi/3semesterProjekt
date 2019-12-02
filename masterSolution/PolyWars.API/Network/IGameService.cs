using Microsoft.AspNet.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace PolyWars.API.Network {
    public interface IGameService {
        event Action<IUser> ClientLoggedIn;
        event Action<string> ClientLoggedOut;
        event Action<string> ClientDisconnected;
        event Action<string> ClientReconnected;
        event Action ConnectionReconnecting;
        event Action ConnectionReconnected;
        event Action ConnectionClosed;
        HubConnection Connection { get; set; }
        Task ConnectAsync();
        Task<IUser> LoginAsync(string name);
        Task LogoutAsync();
    }

}
