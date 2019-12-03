using Microsoft.AspNet.SignalR.Client;
using PolyWars.API.Network.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PolyWars.API.Network {
    public interface IGameService {
        event Action<string> announceClientLoggedIn;
        event Action<string> ClientLoggedOut;
        event Action<string> ClientDisconnected;
        event Action<string> ClientReconnected;
        event Action ConnectionReconnecting;
        event Action ConnectionReconnected;
        event Action ConnectionClosed;
        HubConnection Connection { get; set; }
        Task ConnectAsync();
        Task<IUser> LoginAsync(string name, string hashedPassword);
        Task LogoutAsync();
        Task<List<ResourceDTO>> getResourcesAsync();
    }

}
