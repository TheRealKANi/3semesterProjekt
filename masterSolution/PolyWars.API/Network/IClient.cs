using PolyWars.API.Network.DTO;
using System.Collections.Generic;

namespace PolyWars.API.Network {
    public interface IClient {

        void announceClientLoggedIn(string userName);
        void OnConnected();
        void ClientDisconnected(string name);
        void ClientReconnected(string name);
        void ClientLogout(string name);
        void BroadcastTextMessage(string sender, string message);

        void AccessDenied(string reason);
        void updateOpponents(List<PlayerDTO> opponentDTOs);
    }
}
