﻿using PolyWars.API.Network.DTO;
using PolyWars.Logic;
using System.Collections.Generic;
using System.Diagnostics;

namespace PolyWars.Network {
    static class NetworkController {
        public static bool IsConnected { get; set; }
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
            GameService.updateResources += updateResources;
            GameService.removeResource += removeResource;
            GameService.clientLoggedOut += clientLoggedOut;
            GameService.opponentMoved += opponentMoved;
        }

        private static void opponentMoved(string username, PlayerDTO playerDTO) {
            Adapters.PlayerAdapter.moveOpponentOnCanvas(username, playerDTO);
        }
        private static void clientLoggedOut(string username) {
            //Debug.WriteLine("Server - Recieved Client logged out");
            Adapters.PlayerAdapter.removeOpponentFromCanvas(username);
        }

        private static void removeResource(string resourceID) {
            //Debug.WriteLine("Server - Recieved Resource Removal");
            Adapters.ResourceAdapter.removeResourceFromCanvas(resourceID);
        }

        public static void updateOpponents(List<PlayerDTO> opponentDTOs) {
            //Debug.WriteLine("Server - Recived Opponents Update");
            if(ArenaController.ArenaCanvas != null) {
                GameController.Opponents = Adapters.PlayerAdapter.PlayerDTOtoIShape(opponentDTOs);
            }
        }

        public static void updateResources(List<ResourceDTO> resourceDTOs) {
            //Debug.WriteLine("Server - Recieved Resource Update");
            if(ArenaController.ArenaCanvas != null) {
                GameController.Resources = Adapters.ResourceAdapter.ResourceDTOtoIResource(resourceDTOs);
            }
        }

        public static void announceClientLoggedIn(string userName) {
            Debug.WriteLine($"Server - {userName} has joined the lobby");
        }
        public static void deniedAccess(string reason) {
            Debug.WriteLine($"Server - Access Denied, Reason: {reason}");
        }
    }
}
