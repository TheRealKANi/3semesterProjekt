using PolyWars.API.Network;
using PolyWars.API.Network.DTO;
using System.Collections.Generic;
using System.Diagnostics;

namespace PolyWars.Network {
    static class NetworkController {
        
        static NetworkController() {
            GameService = new GameService();
            //GameService.NewTextMessage += NewTextMessage;
            //GameService.NewImageMessage += NewImageMessage;
            //GameService.ParticipantLoggedIn += ParticipantLogin;
            //GameService.ParticipantLoggedOut += ParticipantDisconnection;
            //GameService.ParticipantDisconnected += ParticipantDisconnection;
            //GameService.ParticipantReconnected += ParticipantReconnection;
            //GameService.ParticipantTyping += ParticipantTyping;
            //GameService.ConnectionReconnecting += Reconnecting;
            //GameService.ConnectionReconnected += Reconnected;
            //GameService.ConnectionClosed += Disconnected;
            GameService.announceClientLoggedIn += announceClientLoggedIn;
            //GameService.OnConnected += announceClientConnected;
            GameService.AccessDenied += DeniedAccess;
        }
        public static GameService GameService { get; private set; }

        public static void announceClientLoggedIn(string userName) {
            Debug.WriteLine($"{userName} has joined the lobby");
        }
        public static void DeniedAccess(string reason) {
            Debug.WriteLine($"Access Denied, Reason: {reason}");
        }
    }
}
