using Microsoft.AspNet.SignalR.Client;
using PolyWars.Adapters;
using PolyWars.Api.Model;
using PolyWars.API;
using PolyWars.API.Model;
using PolyWars.API.Model.Interfaces;
using PolyWars.API.Network;
using PolyWars.API.Network.DTO;
using PolyWars.Logic;
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
        public event Action<PlayerDTO> opponentJoined;
        public event Action<BulletDTO> opponentShoots;
        public event Action<int> updateHealth;
        public event Action<string> playerDied;
        public event Action<BulletDTO> removeBullet;
        public event Action<string> removeDeadOpponent;
        //public event Action<string> OnConnected;

        private IHubProxy hubProxy;
        public HubConnection Connection { get; set; }

        private string serverIP = "localhost"; // Lan Client Test
        //private string serverIP = "109.57.212.47"; // WAN Client Test

        private string protocol = "http://";


        public async Task<bool> ConnectAsync() {
            Connection = new HubConnection(protocol + serverIP + ":" + Constants.serverPort + Constants.serverEndPoint);
            hubProxy = Connection.CreateHubProxy("MainHub");
            hubProxy.On<string>("announceClientLoggedIn", (u) => announceClientLoggedIn?.Invoke(u));
            hubProxy.On<string>("clientLogout", (n) => clientLoggedOut?.Invoke(n));
            hubProxy.On<string>("ClientDisconnected", (n) => ClientDisconnected?.Invoke(n));
            hubProxy.On<string>("ClientReconnected", (n) => ClientReconnected?.Invoke(n));
            //hubProxy.On<string>("OnConnected", (n) => OnConnected?.Invoke(n));

            Connection.Reconnecting += Reconnecting;
            Connection.Reconnected += Reconnected;
            Connection.Closed += Disconnected;

            ServicePointManager.DefaultConnectionLimit = 100;
            await Connection.Start();            
            bool connectionStatus = Connection.State == ConnectionState.Connected ? true : false;

            NetworkController.IsConnected = connectionStatus;
            return connectionStatus;
        }

        

        public void initIngameBindings() {
            hubProxy.On<string>("AccessDenied", (n) => accessDenied?.Invoke(n));
            hubProxy.On<List<PlayerDTO>>("updateOpponents", (lo) => updateOpponents?.Invoke(lo));
            hubProxy.On<List<ResourceDTO>>("updateResources", (lr) => updateResources.Invoke(lr));
            hubProxy.On<string>("removeResource", (rID) => removeResource.Invoke(rID));
            hubProxy.On<PlayerDTO>("opponentMoved", (dto) => opponentMoved.Invoke(dto));
            hubProxy.On<double>("updateWallet", (wallet) => updateWallet.Invoke(wallet));
            hubProxy.On<PlayerDTO>("opponentJoined", (dto) => opponentJoined.Invoke(dto));
            hubProxy.On<BulletDTO>("opponentShoots", (dto) => opponentShoots.Invoke(dto));
            hubProxy.On<int>("updateHealth", (health) => updateHealth.Invoke(health));
            hubProxy.On<string>("playerDied", (killedBy) => playerDied.Invoke(killedBy));
            hubProxy.On<BulletDTO>("removeBullet", (bullet) => removeBullet.Invoke(bullet));
            hubProxy.On<string>("removeDeadOpponent", (username) => removeDeadOpponent.Invoke(username));

        }

        public async Task<bool> playerShoots(int damage) {
            return await hubProxy.Invoke<bool>("playerShoots", damage);
        }
        public async Task<bool> playerGotShot(BulletDTO bullet) {
            return await hubProxy.Invoke<bool>("playerGotShot", bullet);
        }
        public async Task<bool> PlayerMovedAsync(IMoveable playerIRay) {
            PlayerDTO dto = PlayerAdapter.MoveableToPlayerDTO(playerIRay, GameController.Player.Health);
            return await hubProxy.Invoke<bool>("playerMoved", dto); ;
        }
        public async Task<IUser> LoginAsync(string name, string hashedPassword) {
            return await hubProxy.Invoke<User>("Login", name, hashedPassword);
        }

        public async Task<bool> playerCollectedResource(IResource resource) {
            return await hubProxy.Invoke<bool>("playerCollectedResource", resource.ID);
        }

        public async Task LogoutAsync() {
            await hubProxy.Invoke("Logout");
        }        public async Task SendBroadcastMessageAsync(string msg) {
            await hubProxy.Invoke("BroadcastTextMessage", msg);
        }        public async Task<PlayerDTO> getPlayerShip() {
            return await hubProxy.Invoke<PlayerDTO>("getPlayerShip");
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
