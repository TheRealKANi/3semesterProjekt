﻿using Microsoft.AspNet.SignalR.Client;

using PolyWars.API.Model;
using PolyWars.API.Model.Interfaces;
using PolyWars.API.Network;
using PolyWars.API.Network.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;


namespace PolyWars.Network {
    public class GameService : IGameService {

        public event Action<string> announceClientLoggedIn;
        public event Action<string> clientLoggedOut;
        public event Action<string> ClientDisconnected;
        public event Action<string> ClientReconnected;
        public event Action<string> accessDenied;
        public event Action ConnectionReconnecting;
        public event Action ConnectionReconnected;
        public event Action ConnectionClosed;
        public event Action<List<PlayerDTO>> updateOpponents;
        public event Action<List<ResourceDTO>> updateResources;
        public event Action<string> removeResource;
        public event Action<PlayerDTO> opponentMoved;
        public event Action<double> updateWallet; 
        public event Action<BulletDTO> opponentShot;
        //public event Action<string> OnConnected;

        private IHubProxy hubProxy;
        public HubConnection Connection { get; set; }
        //private string url = "http://polywars.servegame.com:8080/Polywars";
        //private string url = "http://192.168.8.15:5700/Polywars"; // Lan k home        //private string url = "http://109.56.76.238:5700/Polywars"; // Wan k home
        private string url = "http://localhost:8080/Polywars";
        public async Task<bool> ConnectAsync() {
            Connection = new HubConnection(url);
            hubProxy = Connection.CreateHubProxy("MainHub");
            hubProxy.On<string>("announceClientLoggedIn", (u) => announceClientLoggedIn?.Invoke(u));
            hubProxy.On<string>("clientLogout", (n) => clientLoggedOut?.Invoke(n));
            hubProxy.On<string>("ClientDisconnected", (n) => ClientDisconnected?.Invoke(n));
            hubProxy.On<string>("ClientReconnected", (n) => ClientReconnected?.Invoke(n));

            //hubProxy.On<string>("OnConnected", (n) => OnConnected?.Invoke(n));
            hubProxy.On<string>("AccessDenied", (n) => accessDenied?.Invoke(n));
            hubProxy.On<List<PlayerDTO>>("updateOpponents", (lo) => updateOpponents?.Invoke(lo));
            hubProxy.On<List<ResourceDTO>>("updateResources", (lr) => updateResources.Invoke(lr));
            hubProxy.On<string>("removeResource", (rID) => removeResource.Invoke(rID));
            hubProxy.On<PlayerDTO>("opponentMoved", (dto) => opponentMoved.Invoke(dto));
            hubProxy.On<double>("updateWallet", (wallet) => updateWallet.Invoke(wallet));
            hubProxy.On<BulletDTO>("opponentShot", (bullet) => opponentShot.Invoke(bullet));

            Connection.Reconnecting += Reconnecting;
            Connection.Reconnected += Reconnected;
            Connection.Closed += Disconnected;

            ServicePointManager.DefaultConnectionLimit = 100;
            await Connection.Start();
            return true;
        }

        public async Task<bool> playerShooted(int amount) {
            return await hubProxy.Invoke<bool>("playerShooted", amount);
        }
        public async Task<bool> PlayerMovedAsync(IRay playerIRay) {
            return await hubProxy.Invoke<bool>("PlayerMoved", playerIRay);
        }
        public async Task<IUser> LoginAsync(string name, string hashedPassword) {
            return await hubProxy.Invoke<User>("Login", name, hashedPassword);
        }

        public async Task<bool> playerCollectedResource(IResource resource) {
            return await hubProxy.Invoke<bool>("playerCollectedResource", resource.ID);
        }

        public async Task LogoutAsync() {
            await hubProxy.Invoke("Logout");
        }

        public async Task SendBroadcastMessageAsync(string msg) {
            await hubProxy.Invoke("BroadcastTextMessage", msg);
        }
        /// <summary>
        /// Grabs the current list of opponents from the server
        /// </summary>
        public async Task<List<PlayerDTO>> getOpponentsAsync() {
            Debug.WriteLine("Client - Asks Server for opponents");
            return await hubProxy.Invoke<List<PlayerDTO>>("getOpponents");
        }
        /// <summary>
        /// Grabs the list of remaning resources from the server
        /// </summary>
        public async Task<List<ResourceDTO>> getResourcesAsync() {
            Debug.WriteLine("Client - Asks Server for resources");
            return await hubProxy.Invoke<List<ResourceDTO>>("getResources");
        }

        public async Task<List<BulletDTO>> getBulletsAsync() {
            Debug.WriteLine("Client - Asks Server for bullets");
            return await hubProxy.Invoke<List<BulletDTO>>("getBullets");
        }

        private void Disconnected() { ConnectionClosed?.Invoke(); }

        private void Reconnected() { ConnectionReconnected?.Invoke(); }

        private void Reconnecting() { ConnectionReconnecting?.Invoke(); }
    }
}
