using PolyWars.API.Network;
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

        // WORKS!
        public static void announceClientLoggedIn(string userName) {
            Debug.WriteLine($"{userName} has joined the lobby");
        }
        //// Works!
        //public static void announceClientConnected(string name) {
        //    Debug.WriteLine(name);
        //}

        // Works!
        public static void DeniedAccess(string reason) {
            Debug.WriteLine($"Access Denied, Reason: {reason}");
        }
    }
}
