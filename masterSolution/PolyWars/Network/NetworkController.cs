﻿using PolyWars.Adapters;
using PolyWars.Api.Model;
using PolyWars.API.Model.Interfaces;
using PolyWars.API.Network.DTO;
using PolyWars.Logic;
using PolyWars.ServerClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PolyWars.Network {
    static class NetworkController {
        private static bool isConnected;
        public static bool IsConnected {
            get {
                return isConnected;
            }
            set {
                isConnected = value;
                UIDispatcher.Invoke(() => CommandManager.InvalidateRequerySuggested());
            }
        }
        public static GameService GameService { get; private set; }

        static NetworkController() {
            GameService = new GameService();

            GameService.announceClientLoggedIn += announceClientLoggedIn;
            //GameService.accessDenied += deniedAccess;
            //GameService.updateOpponents += updateOpponents;
            //GameService.updateResources += updateResources;
            GameService.removeResource += removeResource;
            GameService.clientLoggedOut += clientLoggedOut;
            GameService.opponentMoved += opponentMoved;
            GameService.updateWallet += updateWallet;
            GameService.opponentJoined += opponentJoined;
            GameService.opponentShot += opponentShot;
        }
        private static void opponentShot(BulletDTO bullet) {
            Bullet newBullet = BulletAdapter.renderBullet(bullet);
            if(GameController.Bullets.TryAdd(newBullet.ID, newBullet)) {
                BulletAdapter.addBulletToCanvas(newBullet);
            }
        }
        private static void updateWallet(double walletAmount) {
            GameController.Player.Wallet += walletAmount;
        }
        private static void opponentJoined(PlayerDTO dto) {
            if(!GameController.Opponents.ContainsKey(dto.Name)) {
                IMoveable opponent = Adapters.PlayerAdapter.playerDTOToMoveable(dto);
                bool succeded = GameController.Opponents.TryAdd(dto.Name, opponent);
                if(succeded) {
                    UIDispatcher.Invoke(() => ArenaController.ArenaCanvas.Children.Add(opponent.Shape.Polygon));
                }
            }
        }
        private static void opponentMoved(PlayerDTO dto) {
            if(GameController.Opponents.ContainsKey(dto.Name)) {
                IMoveable opponent = GameController.Opponents[dto.Name];
                opponent.Velocity = dto.Velocity;
                opponent.MaxVelocity = dto.MaxVelocity;
                opponent.RPM = dto.RPM;
                opponent.MaxRPM = dto.MaxRPM;

                IRay ray = new Ray(opponent.Shape.Ray.ID, new Point(dto.centerX, dto.centerY), dto.Angle);
                opponent.Shape.Ray = ray;
            }
        }
        private static void clientLoggedOut(string id) {
            if(GameController.Opponents.ContainsKey(id)) {
                IMoveable opponent;
                while(!GameController.Opponents.TryRemove(id, out opponent)) {
                    Task.Delay(5);
                }
                UIDispatcher.Invoke(() => ArenaController.ArenaCanvas.Children.Remove(opponent.Shape.Polygon));
            }
        }
        private static void removeResource(string id) {
            //Debug.WriteLine("Server - Recieved Resource Removal");
            if(GameController.Resources.ContainsKey(id)) {
                IResource resource;
                while(!GameController.Resources.TryRemove(id, out resource)) {
                    Task.Delay(5);
                }
                UIDispatcher.Invoke(() => ArenaController.ArenaCanvas.Children.Remove(resource.Shape.Polygon));
            }
        }

        //public static void updateOpponents(List<PlayerDTO> opponentDTOs) {
        //    //Debug.WriteLine("Server - Recived Opponents Update");
        //    if(ArenaController.ArenaCanvas != null) {
        //        GameController.Opponents = Adapters.PlayerAdapter.PlayerDTOtoIShape(opponentDTOs);
        //    }
        //}

        //public static void updateResources(List<ResourceDTO> resourceDTOs) {
        //    //Debug.WriteLine("Server - Recieved Resource Update");
        //    if(ArenaController.ArenaCanvas != null) {
        //        GameController.Resources = Adapters.ResourceAdapter.ResourceDTOtoIResource(resourceDTOs);
        //    }
        //}

        public static void announceClientLoggedIn(string userName) {
            Debug.WriteLine($"Server - {userName} has joined the lobby");
        }

        private static void updateHealth(int healthLeft) {
            GameController.Player.Health = healthLeft;
            //Debug.WriteLine("Server - Recieved health update");
        }
    }
}
