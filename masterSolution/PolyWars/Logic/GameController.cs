using PolyWars.Api;
using PolyWars.Api.Model;
using PolyWars.API;
using PolyWars.API.Model.Interfaces;
using PolyWars.API.Strategies;
using PolyWars.Logic.Utility;
using PolyWars.Model;
using PolyWars.Network;
using PolyWars.ServerClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Collections.Concurrent;
using PolyWars.API.Network.DTO;
using PolyWars.Adapters;

namespace PolyWars.Logic {
    class GameController {

        private static bool isPrepared;
        private static int frames = 0;
        private static Stopwatch fpsTimer;
        private static IRay lastRay;
        public static Ticker Ticker { get; private set; }
        public static IPlayer Player { get; set; }
        public static string Username { get; set; }
        public static string UserID { get; set; }
        public static ConcurrentDictionary<string, IMoveable> Opponents { get; set; }
        public static ConcurrentDictionary<string, IResource> Resources { get; set; }
        public static ConcurrentDictionary<string, IBullet> Bullets { get; set; }
        public static int Fps { get; set; }
        public static Stopwatch tickTimer { get; private set; }
        private static Stopwatch ServerTimer { get; set; }
        public static double ArenaWidth { get; set; }
        public static double ArenaHeight { get; set; }
        public static bool IsPlayerDead { get; set; }

        private static long timeStamp;
        private static long lastStamp;

        private const decimal baselineFps = 1000m / 60; // miliseconds per frame at 60 fps 

        static public EventHandler<EventArgs> CanvasChangedEventHandler;
        private static bool serverResponded;



        /// <summary>
        /// Default constructor of GameController Class
        /// </summary>
        static GameController() {
            timeStamp = 0;
            serverResponded = true;
            isPrepared = false;
            fpsTimer = new Stopwatch();
            fpsTimer.Reset();
            Ticker = new Ticker();
            tickTimer = new Stopwatch();
            ServerTimer = new Stopwatch();
            ServerTimer.Start();
        }

        public void prepareGame() {
            // instanciate
            ArenaController.generateCanvas();
            Bullets = new ConcurrentDictionary<string, IBullet>();
            Opponents = new ConcurrentDictionary<string, IMoveable>();
            Resources = new ConcurrentDictionary<string, IResource>();

            PlayerDTO playerDTO = null;
            IList<PlayerDTO> opponentDTOs = new List<PlayerDTO>();
            IList<ResourceDTO> resourceDTOs = new List<ResourceDTO>();
            Task[] taskPool = new Task[3];

            // get game objects from the server
            taskPool[0] = Task.Run(async () => playerDTO = await NetworkController.GameService.getPlayerShip());
            taskPool[1] = Task.Run(async () => opponentDTOs = await NetworkController.GameService.getOpponentsAsync());
            taskPool[2] = Task.Run(async () => resourceDTOs = await NetworkController.GameService.getResourcesAsync());

            Task.WaitAll(taskPool);

            // create the player
            IMoveable playerShip = PlayerAdapter.playerDTOToMoveable(playerDTO);
            playerShip.Shape.Renderable.BorderColor = Colors.Black;
            playerShip.Shape.Renderable.FillColor = Colors.Gray;
            playerShip.Mover = new MoveStrategy();
            UIDispatcher.Invoke(() => { Player = new Player(Username, UserID, playerDTO.Wallet, playerDTO.Health, playerShip); });

            // convert data transfer objects to their respective types and add them to list
            foreach(PlayerDTO opponent in opponentDTOs) {
                IMoveable moveable = PlayerAdapter.playerDTOToMoveable(playerDTO);
                while(!Opponents.TryAdd(opponent.Name, moveable)) {
                    Task.Delay(1);
                }
            }
            foreach(ResourceDTO resource in resourceDTOs) {
                IResource r = ResourceAdapter.DTOToResource(resource);
                while(!Resources.TryAdd(resource.ID, r)) {
                    Task.Delay(1);
                }
            }

            // add objects to the canvas
            UIDispatcher.Invoke(() => {
                foreach(IResource resource in Resources.Values) {
                    ArenaController.ArenaCanvas.Children.Add(resource.Shape.Polygon);
                }
            });
            UIDispatcher.Invoke(() => ArenaController.ArenaCanvas.Children.Add(Player.PlayerShip.Shape.Polygon));
            UIDispatcher.Invoke(() => {
                foreach(IMoveable opponent in Opponents.Values) {
                    ArenaController.ArenaCanvas.Children.Add(opponent.Shape.Polygon);
                }
            });
            isPrepared = true;
        }

        public void playGame() {
            IsPlayerDead = false;
            if(isPrepared) {
                fpsTimer.Start();
                Ticker.Start();
            }
        }

        public void endGame() {
            Ticker.Stop();
            fpsTimer.Stop();
        }

        public static decimal DeltaTime(Stopwatch _tickTimer) {
            return (decimal) _tickTimer.Elapsed.TotalMilliseconds / baselineFps;
        }

        static public void calculateFrame() {
            try {
                decimal deltaTime = DeltaTime(tickTimer);
                Player.PlayerShip.Move(deltaTime);
                UIDispatcher.Invoke(() => {
                    Task.Run(() => notifyMoved());
                    foreach(IMoveable opponent in Opponents.Values) {
                        opponent.Move(deltaTime);
                    }
                    foreach(IBullet bullet in Bullets.Values) {
                        Point p = bullet.BulletShip.Shape.Ray.CenterPoint;
                        if(bulletOutOfBounds(p)) {
                            BulletAdapter.removeBulletFromCanvas(bullet.ID);
                            Bullets.TryRemove(bullet.ID, out IBullet bulletOut);
                        } else {
                            bullet.BulletShip.Move(DeltaTime(tickTimer));
                        }
                    }
                    tickTimer.Stop();
                    CanvasChangedEventHandler?.Invoke(null, EventArgs.Empty);
                });
            } catch(TaskCanceledException) {
                // TODO Should we do something here
            }
        }

        /// <summary>
        ///     Checks if a bullet is trying to go past the bounds
        /// </summary>
        private static bool bulletOutOfBounds(Point p) {
            bool result = true;
            int upperWidthBound = 4000; // Ish 4k 
            int upperHeightBound = 2200;

            int lowerHeightBound = 0;
            int lowerWidthBound = 0;

            if(p.X > lowerWidthBound && p.X <= upperWidthBound) {
                if(p.Y > lowerHeightBound && p.Y <= upperHeightBound) {
                    result = false;
                }
            }
            return result;
        }

        public static async void notifyMoved() {

            if((timeStamp = Stopwatch.GetTimestamp()) - lastStamp > 70) { // ish 100 times a second
                //serverResponded = false;
                ServerTimer.Restart();
                //serverResponded = await UIDispatcher.Invoke(() => NetworkController.GameService.PlayerMovedAsync(Player.PlayerShip));
                UIDispatcher.Invoke( () => NetworkController.GameService.PlayerMovedAsync(Player.PlayerShip));
                lastRay = ((Ray) Player.PlayerShip.Shape.Ray).Clone();
                lastStamp = timeStamp;
            }
        }
        static public void calculateFps() {
            try {
                frames++;
                if(fpsTimer.Elapsed.TotalMilliseconds >= 1000) {
                    Fps = frames;
                    frames = 0;
                    fpsTimer.Restart();

                }
            } catch(TaskCanceledException) {
                // TODO Do we need to handle this?
            }
        }
    }
}