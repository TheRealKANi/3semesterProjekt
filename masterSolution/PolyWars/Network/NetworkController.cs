using PolyWars.API.Model.Interfaces;
using PolyWars.API.Network.DTO;
using PolyWars.Logic;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PolyWars.Network {
    static class NetworkController {
        public static bool IsConnected { get; set; }
        private static void resourcesChanged(List<ResourceDTO> obj) {
            throw new NotImplementedException();
        }
        public static GameService GameService { get; private set; }

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
            GameService.accessDenied += deniedAccess;
            GameService.updateOpponents += updateOpponents;
        }

        public static void updateOpponents(List<PlayerDTO> opponentDTOs) {
            //Debug.WriteLine("Recived Opponents Update");
            if(ArenaController.ArenaCanvas != null) { 
                GameController.Opponents = Adapters.PlayerAdapter.PlayerDTOtoIShape(opponentDTOs); 
            }
        }


        public static void announceClientLoggedIn(string userName) {
            Debug.WriteLine($"{userName} has joined the lobby");
        }
        public static void deniedAccess(string reason) {
            Debug.WriteLine($"Access Denied, Reason: {reason}");
        }
    }
}
