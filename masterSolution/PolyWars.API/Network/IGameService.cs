using Microsoft.AspNet.SignalR.Client;
using PolyWars.API.Network.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PolyWars.API.Network {
    public interface IGameService {
        event Action<string> clientLoggedOut;
        event Action<string> announceClientLoggedIn;
        event Action<string> ClientDisconnected;
        event Action<string> ClientReconnected;
        event Action<string> accessDenied;
        event Action ConnectionReconnecting;
        event Action ConnectionReconnected;
        event Action ConnectionClosed;
        event Action<List<PlayerDTO>> updateOpponents;
        event Action<List<ResourceDTO>> updateResources;
        event Action<string> removeResource;
        event Action<PlayerDTO> opponentMoved;

        HubConnection Connection { get; set; }
        Task<bool> ConnectAsync();
        Task<IUser> LoginAsync(string name, string hashedPassword);
        Task LogoutAsync();
        Task<List<ResourceDTO>> getResourcesAsync();
        Task<List<PlayerDTO>> getOpponentsAsync();
        Task<List<BulletDTO>> getBulletsAsync();
    }

}
