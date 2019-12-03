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
        //private static ConcurrentDictionary<string, IUser> ActiveShot = new ConcurrentDictionary<string, IUser>();

        static Stopwatch s;
        private static int count;
        static MainHub() {
            PlayerClients = new ConcurrentDictionary<string, IUser>();
            Opponents = new ConcurrentDictionary<string, PlayerDTO>();
            Resources = new ConcurrentDictionary<string, ResourceDTO>();

            IEnumerable<ResourceDTO> resources = ResourceFactory.generateResources(50, 1);
            foreach(ResourceDTO resource in resources) {
                Resources.TryAdd(resource.Ray.ID, resource);
            }
        }
        //public bool PlayerCollectedResource(string resourceId) {
        //    ResourceDTO r;
        //    bool removed = Resources.TryRemove(resourceId, out r);

        //    if(removed) {
        //        //TODO Clients.Caller.AddcurrencyToWallet(r.Value);
        //    }

        //    return removed;
        //}
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

        public override Task OnConnected() {
            Console.WriteLine($"Client connected: '{Context.ConnectionId}'");
            return base.OnConnected();
        }

        // TODO should get user from database
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
                        Ray = new Ray(newUser.ID, new Point(r.Next(50, 400), 300), 0), // Needs ray
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
            
            // Verify that IRay is not beyond movement bounds
            //Console.WriteLine($"Recived IRay from client: '{Clients.CallerState.UserName}'");
            //Console.WriteLine($"IRay: {playerIray.ToString()}");
            //Console.WriteLine(s.Elapsed.TotalMilliseconds);
            bool result = false;
            //// Method 1 - Too complex
            //PlayerDTO orginal = new PlayerDTO();
            //PlayerDTO newUser = new PlayerDTO();
            //Opponents.TryGetValue(Clients.CallerState.UserName, out orginal);
            //if(orginal.ID.Length > 0) {
            //    newUser.ID = orginal.ID;
            //    newUser.Name = orginal.Name;
            //    newUser.Vertices = orginal.Vertices;
            //    newUser.Wallet = orginal.Wallet;
            //    newUser.Ray = playerIRay;
            //    Opponents.TryUpdate(newUser.Name, newUser, orginal);
            //    Console.WriteLine("Updated Opponent with new Ray");
            //    result = true;
            //}

            // Method 2 - Better
            string name = Clients.CallerState.UserName;
            Opponents.TryRemove(name, out PlayerDTO player);
            if(player != null) {
                player.Ray = playerIRay;
                result = Opponents.TryAdd(name, player);
            }

            if(s == null) {
                s = new Stopwatch();
                s.Start();
                count = 0;
            }
            count++;
            if(s.Elapsed.TotalMilliseconds >= 1000) {
                s.Stop();
                Console.WriteLine($"Getting {count} PlayerMoved counts pr. second");
                s.Restart();
                count = 0;
                List<PlayerDTO> opponents = new List<PlayerDTO>(Opponents.Values);
                Console.WriteLine("Sending Opponents to all clients once pr. second");
                Clients.All.updateOpponents(opponents);
            }
            return result;
        }

        public void Logout() {
            string name = Clients.CallerState.UserName;
            if(!string.IsNullOrEmpty(name)) {
                PlayerClients.TryRemove(name, out IUser client);
                Clients.Others.ClientLogout(name);
                Console.WriteLine($"-- {name} logged out");
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
