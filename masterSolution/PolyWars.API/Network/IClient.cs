using PolyWars.API.Network.DTO;
using System.Collections.Generic;
namespace PolyWars.API.Network {
    /// <summary>
    /// Base class for server to know which methods
    /// are available on a client of the hub
    /// </summary>
    public interface IClient {
        void OnConnected();
        void ClientDisconnected(string name);
        void ClientReconnected(string name);
        void announceClientLoggedIn(string userName);
        void clientLogout(string name);
        void BroadcastTextMessage(string sender, string message);
        void AccessDenied(string reason);
        void updateOpponents(List<PlayerDTO> opponentDTOs);
        void updateResources(List<ResourceDTO> resourcesDTOs);
        void removeResource(string removedResourceID);
        void opponentMoved(PlayerDTO playerDTO);
        void updateWallet(double wallet);
        void opponentShoots(BulletDTO bullet);
        void opponentJoined(PlayerDTO dto);
        void updateHealth(int health);
        void playerDied(string playerID);
        void removeBullet(BulletDTO bulletDTO);
        void removeDeadOpponent(string userName);
    }
}