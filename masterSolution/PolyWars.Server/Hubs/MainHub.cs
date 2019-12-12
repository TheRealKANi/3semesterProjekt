using Microsoft.AspNet.SignalR;
using PolyWars.API;
using PolyWars.API.Network;
using PolyWars.API.Network.DTO;
using PolyWars.Server.Factories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;using System.Threading.Tasks;
using System.Windows.Media;

namespace PolyWars.Server {
    public class MainHub : Hub<IClient> {
        private static ConcurrentDictionary<string, IUser> PlayerClients;
        private static ConcurrentDictionary<string, PlayerDTO> Opponents;
        private static ConcurrentDictionary<string, ResourceDTO> Resources;
        private static ConcurrentDictionary<string, BulletDTO> Bullets;
        private static ConcurrentDictionary<string, int> statistics;
        private static object statisticsLock = new object(); 
        private static Random rnd;



        // System.Windows.Media.Colors.DarkSlateGray

        private static Stopwatch statisticTimer;

        private void methodCallCounter([CallerMemberName] string method = "") {
            if(!string.IsNullOrWhiteSpace(method)) {
                if(statistics.ContainsKey(method)) {
                    statistics[method]++;
                } else {
                    try {
                        statistics.TryAdd(method, 1);
                    } catch(Exception e) {
                        Console.WriteLine("Try add methodCallCounter execption: " + e.Message);
                    }
                }
            }
            if(statisticTimer.Elapsed.TotalSeconds > 9) {
                int x = Console.CursorLeft;
                int y = Console.CursorTop;
                statisticTimer.Restart();
                Console.Clear();
                Console.SetCursorPosition(0, 1);
                Console.WriteLine("****************************************************************************                                       ");
                foreach(string key in statistics.Keys) {
                    Console.WriteLine($"{key,-30} was called {statistics[key],-6} times                                                               ");
                }
                Console.WriteLine("****************************************************************************                                        ");
                Console.SetCursorPosition(x, 13);
            }
        }



        static MainHub() {
            rnd = new Random((int)Stopwatch.GetTimestamp());
            Console.SetCursorPosition(0, 1);
            PlayerClients = new ConcurrentDictionary<string, IUser>();
            Opponents = new ConcurrentDictionary<string, PlayerDTO>();
            Resources = new ConcurrentDictionary<string, ResourceDTO>();
            Bullets = new ConcurrentDictionary<string, BulletDTO>();
            statistics = new ConcurrentDictionary<string, int>();
            statisticTimer = new Stopwatch();
            statistics.TryAdd("moved stack", 0);
            //statisticTimer.Start();

            IEnumerable<ResourceDTO> resources = ResourceFactory.generateResources(1000, 1);
            foreach(ResourceDTO resource in resources) {
                Resources.TryAdd(resource.ID, resource);
            }
        }

        public bool playerGotShot(BulletDTO bullet) {
            methodCallCounter();
            bool result = false;
            if(Opponents.ContainsKey(Clients.CallerState.UserName)) {
                PlayerDTO player = Opponents[Clients.CallerState.UserName];
                if(Bullets.ContainsKey(bullet.ID) && Bullets.TryRemove(bullet.ID, out BulletDTO bulletDTO)) {
                    Clients.Others.removeBullet(bulletDTO);
                    player.Health -= bulletDTO.Damage;
                    result = true;
                    if(player.Health < 1) {
                        if(Opponents.TryRemove(player.Name, out PlayerDTO deadPlayer)) {
                            Clients.Caller.playerDied(bulletDTO.PlayerID);
                            Clients.Others.removeDeadOpponent(deadPlayer.Name);
                        } else {
                            Console.WriteLine($"playerGotShot Error, Could not remove dead player '{deadPlayer.Name}' from List of Opponents");
                        }
                    } else {
                        Clients.Caller.updateHealth(player.Health);
                    }
                }
            }
            return result;
        }

        public async Task<bool> playerShoots(int damage) {
            methodCallCounter();
            bool result = false;
            PlayerDTO shootingPlayer = Opponents[Clients.CallerState.UserName];
            await Task.Factory.StartNew(() => {
                if(shootingPlayer != null) {
                    BulletDTO bullet = BulletFactory.generateBullet(damage, shootingPlayer);
                    bool addBullet = Bullets.TryAdd(bullet.ID, bullet);
                    if(addBullet) {
                        result = true;
                        Clients.All.opponentShoots(bullet);
                    }
                }
            });
            return result;
        }

        // Called from client when they collide with a resource
        public async Task<bool> playerCollectedResource(string resourceId) {
            methodCallCounter();
            bool removed = false;
            await Task.Factory.StartNew(() => {
                removed = Resources.TryRemove(resourceId, out ResourceDTO r);
                if(removed) {
                    Clients.Others.removeResource(resourceId);
                    string username = Clients.CallerState.UserName;
                    Opponents[username].Wallet += r.Value;
                    Clients.Caller.updateWallet(Opponents[username].Wallet);
                    removed = true;
                }
            });
            return removed;
        }

        public async Task<List<ResourceDTO>> getResources() {
            methodCallCounter();
            List<ResourceDTO> resources = await Task.Factory.StartNew(() => new List<ResourceDTO>(Resources.Values));
            return resources;
        }
        /// <summary>
        /// Returns a list with opponents on the server AND containing the client's own object
        /// </summary>
        public async Task<List<PlayerDTO>> getOpponents() {
            methodCallCounter();
            List<PlayerDTO> opponents = await Task.Factory.StartNew(() => new List<PlayerDTO>(Opponents.Values.Where(x => x.Name != Clients.CallerState.UserName)));
            return opponents;
        }        public async Task<PlayerDTO> getPlayerShip() {
            methodCallCounter();
            string userName = Clients.CallerState.UserName;
            return await Task.Factory.StartNew<PlayerDTO>(() => {
                if(Opponents.ContainsKey(userName)) {
                    return Opponents[Clients.CallerState.UserName];
                }
                return null;
            });
        }

        public List<BulletDTO> getBullets() {
            methodCallCounter();
            List<BulletDTO> bullets = new List<BulletDTO>(Bullets.Values);
            return bullets;
        }

        public override Task OnConnected() {
            methodCallCounter();
            Console.WriteLine($"Client connected: '{Context.ConnectionId}'");
            return base.OnConnected();
        }
        public async Task<IUser> Login(string username, string hashedPassword) {
            methodCallCounter();
            return await Task.Factory.StartNew(() => {
                if(!PlayerClients.ContainsKey(username)) {
                    // TODO Verify user creds from DB here
                    if(true) {
                        Console.WriteLine($"++ {username} logged in");
                        IUser newUser = new User(username, Context.ConnectionId, hashedPassword);
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
                        PlayerDTO newPlayer = new PlayerDTO() {
                            ID = newUser.ID,
                            Name = newUser.Name,
                            centerX = r.Next(50, 500),
                            centerY = r.Next(50, 500),
                            Angle = 0,
                            Velocity = 0,
                            MaxVelocity = 15,
                            RPM = 0,
                            MaxRPM = 60,
                            Vertices = 3,
                            Wallet = 0,
                            Width = 50,
                            Height = 50,
                            Health = 100,
                            FillColor = GetColor(newUser.Name)
                        };
                        if(Opponents.TryAdd(newPlayer.Name, newPlayer)) {
                            Clients.Others.opponentJoined(newPlayer);
                            return newUser;
                        }
                    } /*else {
                    // Handle what to send back to denied client
                    Clients.Caller.AccessDenied("No way Jose!");
                }*/
                }
                return null;
            });
        }
        private static Color GetColor(string username) {
            Color c = Color.FromArgb(255, (byte) rnd.Next(64, 256), (byte) rnd.Next(64, 256), (byte) rnd.Next(64, 256));
            return c;
        }
        public async Task<bool> playerMoved(PlayerDTO player) {
            statistics["moved stack"]++;

            methodCallCounter();
            // Move player in table and transmit new location to other clients
            bool result = false;
            Task t = Task.Factory.StartNew(() => {
                if(Opponents.ContainsKey(player.Name)) {
                    if(Opponents.TryUpdate(player.Name, player, Opponents[player.Name])) {
                        Clients.Others.opponentMoved(player);
                        result = true;
                    }
                }
            });
            await t;
            statistics["moved stack"]--;
            return result;
        }

        public async Task Logout() {
            methodCallCounter();
            string username = Clients.CallerState.UserName;
            await Task.Factory.StartNew(() => {
                if(!string.IsNullOrEmpty(username)) {
                    if(Opponents.TryRemove(username, out PlayerDTO player)) {
                        PlayerClients.TryRemove(username, out IUser client);
                        Clients.Others.clientLogout(username);
                        Console.WriteLine($"-- {username} logged out");
                    }
                }
            });
        }
        public override async Task OnReconnected() {
            methodCallCounter();
            string userName = PlayerClients.SingleOrDefault((c) => c.Value.ID == Context.ConnectionId).Key;

            if(userName != null) {
                Clients.Others.ClientReconnected(userName);
                Console.WriteLine($"== {userName} reconnected");
            }

            await base.OnReconnected();
        }
        public override Task OnDisconnected(bool stopCalled) {
            methodCallCounter();
            string userName = PlayerClients.SingleOrDefault((c) => c.Value.ID == Context.ConnectionId).Key;
            if(userName != null) {
                Clients.Others.ClientDisconnected(userName);
                Console.WriteLine($"<> {userName} disconnected");
            }
            return base.OnDisconnected(stopCalled);
        }
    }
}
