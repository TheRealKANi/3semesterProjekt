using Microsoft.AspNet.SignalR;
using PolyWars.Api.Model;
using PolyWars.API;
using PolyWars.API.Network;
using PolyWars.API.Network.DTO;
using PolyWars.Server.Factories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PolyWars.Server {
    public class MainHub : Hub<IClient> {
        private static ConcurrentDictionary<string, IUser> PlayerClients;
        private static ConcurrentDictionary<string, PlayerDTO> Opponents;
        private static ConcurrentDictionary<string, ResourceDTO> Resources;
        private static ConcurrentDictionary<string, BulletDTO> Bullets;
        //private static ConcurrentDictionary<string, IUser> ActiveShot = new ConcurrentDictionary<string, IUser>();

        static Stopwatch s;
        private static int count;
        static MainHub() {
            PlayerClients = new ConcurrentDictionary<string, IUser>();
            Opponents = new ConcurrentDictionary<string, PlayerDTO>();
            Resources = new ConcurrentDictionary<string, ResourceDTO>();
            Bullets = new ConcurrentDictionary<string, BulletDTO>();

            IEnumerable<ResourceDTO> resources = ResourceFactory.generateResources(100, 1);
            foreach(ResourceDTO resource in resources) {
                Resources.TryAdd(resource.ID, resource);
            }
        }

        public bool playerShooted(int amount) {
            bool result = false;
            PlayerDTO shootingPlayer = Opponents[Clients.CallerState.UserName];
            if(shootingPlayer != null) {
                BulletDTO bullet = BulletFactory.generateBullet(amount, shootingPlayer);
                bool addBullet = Bullets.TryAdd(bullet.ID, bullet);
                if(addBullet) {
                    result = true;
                    Clients.All.opponentShot(bullet);
                }
            }
            return result;
        }

        // Called from client when they collide with a resource
        public bool playerCollectedResource(string resourceId) {
            bool removed = Resources.TryRemove(resourceId, out ResourceDTO r);
            if(removed) {
                Console.WriteLine($"Player Removed Resource: {resourceId}");
                // Updates all other clients with new list of resources
                Clients.Others.removeResource(resourceId);
                string username = Clients.CallerState.UserName;
                Opponents[username].Wallet += r.Value;
                Clients.Caller.updateWallet(Opponents[username].Wallet);
                return true;
            }
            return removed;
        }

        public List<ResourceDTO> getResources() {
            List<ResourceDTO> resources = new List<ResourceDTO>(Resources.Values);
            Console.WriteLine($"Client asked for resources: '{Context.ConnectionId}'");
            return resources;
        }
        /// <summary>
        /// Returns a list with opponents on the server AND containing the client's own object
        /// </summary>
        public List<PlayerDTO> getOpponents() {
            List<PlayerDTO> opponents = new List<PlayerDTO>(Opponents.Values);
            Console.WriteLine($"Client asked for opponents: '{Context.ConnectionId}'");
            return opponents;
        }

        public List<BulletDTO> getBullets() {
            List<BulletDTO> bullets = new List<BulletDTO>(Bullets.Values);
            Console.WriteLine($"Client asked for bullets: '{Context.ConnectionId}'");
            return bullets;
        }

        public override Task OnConnected() {
            Console.WriteLine($"Client connected: '{Context.ConnectionId}'");
            return base.OnConnected();
        }
        public User Login(string username, string hashedPassword) {
            if(!PlayerClients.ContainsKey(username)) {

                // TODO Verify user creds from DB here
                if(true) {
                    Console.WriteLine($"++ {username} logged in on connection id: '{Context.ConnectionId}', pass :'{hashedPassword}'");
                    User newUser = new User(username, Context.ConnectionId, hashedPassword);
                    bool added = PlayerClients.TryAdd(username, newUser);

                    if(!added) {
                        return null;
                    }
                    // Keeps username in shared state until client logs out
                    Clients.CallerState.UserName = username;

                    // Accounces to all other connected clients that *username* has joined
                    Clients.Others.announceClientLoggedIn(username);
                    Random r = new Random();
                    // Creates the basic opponent layout
                    Opponents.TryAdd(username, new PlayerDTO() {
                        ID = newUser.ID,
                        Name = newUser.Name,
                        Ray = new Ray(newUser.ID, new Point(r.Next(50, 400), 300), 0),
                        Vertices = 3,
                        Wallet = 0
                    });

                    return newUser;
                } /*else {
                    // Handle what to send back to denied client
                    Clients.Caller.AccessDenied("No way Jose!");
                }*/
            }
            return null;
        }


        public bool PlayerMoved(Ray playerIRay) {
            bool result = false;
            string username = Clients.CallerState.UserName;
            // Move player in table and transmit new location to other clients
            if(Opponents.TryRemove(username, out PlayerDTO playerDTO)) {
                playerDTO.Ray = playerIRay;
                if(Opponents.TryAdd(username, playerDTO)) {
                    Clients.Others.opponentMoved(playerDTO);
                    result = true;
                }
            }
            return result;
        }

        public void Logout() {
            string username = Clients.CallerState.UserName;
            if(!string.IsNullOrEmpty(username)) {
                if(Opponents.TryRemove(username, out PlayerDTO player)) {
                    PlayerClients.TryRemove(username, out IUser client);
                    Clients.Others.clientLogout(username);
                    Console.WriteLine($"-- {username} logged out");
                }
            }
        }
        public override Task OnReconnected() {
            string userName = PlayerClients.SingleOrDefault((c) => c.Value.ID == Context.ConnectionId).Key;
            if(userName != null) {
                Clients.Others.ClientReconnected(userName);
                Console.WriteLine($"== {userName} reconnected");
            }
            return base.OnReconnected();
        }
        public override Task OnDisconnected(bool stopCalled) {
            string userName = PlayerClients.SingleOrDefault((c) => c.Value.ID == Context.ConnectionId).Key;
            if(userName != null) {
                Clients.Others.ClientDisconnected(userName);
                Console.WriteLine($"<> {userName} disconnected");
            }
            return base.OnDisconnected(stopCalled);
        }
    }
}
