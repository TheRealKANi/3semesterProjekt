using PolyWars.Adapters;
using PolyWars.API.Model;
using PolyWars.API.Model.Interfaces;
using PolyWars.API.Network.DTO;
using PolyWars.Client.Model;
using PolyWars.Network;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace PolyWars.Client.Logic {
    class GameController {
        public static IPlayer Player { get; set; }
        public static string Username { get; set; }
        public static string UserID { get; set; }
        public static bool IsPlayerDead { get; set; }

        public static Ticker Ticker { get; private set; }
        public static Stopwatch tickTimer { get; private set; }
        public static int Fps { get; set; }
        public static bool DebugFrameTimings { get; internal set; }
        public static double ArenaWidth { get; set; }
        public static double ArenaHeight { get; set; }
        public static EventHandler<EventArgs> CanvasChangedEventHandler { get; set; }

        public static ConcurrentDictionary<string, IMoveable> Opponents { get; set; }
        public static ConcurrentDictionary<string, IResource> Resources { get; set; }
        public static ConcurrentDictionary<string, IBullet> Bullets { get; set; }

        private static Stopwatch ServerTimer { get; set; }
        private static bool isPrepared;
        private static int frames = 0;
        private static Stopwatch fpsTimer;
        private static IRay lastPlayerRay;

        static GameController() {
            DebugFrameTimings = false; // Enable/Disable Frame timings Debug output via F3 on keyboard
            isPrepared = false;
            fpsTimer = new Stopwatch();
            fpsTimer.Reset();
            Ticker = new Ticker();
            tickTimer = new Stopwatch();
            ServerTimer = new Stopwatch();
            ServerTimer.Start();
        }

        /// <summary>
        /// Main prepareation logic
        /// </summary>
        public void prepareGame() {
            // instanciate
            ArenaController.ArenaBoundWidth = 1024;
            ArenaController.ArenaBoundHeight = 768;
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

            setupDTOs(playerDTO, opponentDTOs, resourceDTOs);
            addPolygonsToArena();
            isPrepared = true;
        }

        private static void setupDTOs(PlayerDTO playerDTO, IList<PlayerDTO> opponentDTOs, IList<ResourceDTO> resourceDTOs) {
            // create the player
            IMoveable playerShip = PlayerAdapter.playerDTOToMoveable(playerDTO, Colors.Black);
            playerShip.Mover = new MoveStrategy();
            UIDispatcher.Invoke(() => { Player = new Player(Username, UserID, playerDTO.Wallet, playerDTO.Health, playerShip); });

            // convert data transfer objects to their respective types and add them to list
            foreach(PlayerDTO opponent in opponentDTOs) {
                IMoveable moveable = PlayerAdapter.playerDTOToMoveable(opponent, Colors.Red);
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
        }

        private static void addPolygonsToArena() {
            // add objects to the canvas
            UIDispatcher.Invoke(() => {
                foreach(IMoveable opponent in Opponents.Values) {
                    ArenaController.ArenaCanvas.Children.Add(opponent.Shape.Polygon);
                }
            });
            UIDispatcher.Invoke(() => {
                foreach(IResource resource in Resources.Values) {
                    ArenaController.ArenaCanvas.Children.Add(resource.Shape.Polygon);
                }
            });
            UIDispatcher.Invoke(() => ArenaController.ArenaCanvas.Children.Add(Player.PlayerShip.Shape.Polygon));
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

        /// <summary>
        /// Main game logic for controlling everything
        /// </summary>
        /// <param name="deltaTime">The timefactor to consider</param>
        static public void calculateFrame(double deltaTime) {
            try {
                Player.PlayerShip.Move(deltaTime);
                Task.Run(() => notifyMoved());
                runGameLoopTasks(deltaTime);
                tickTimer.Stop();
                CanvasChangedEventHandler?.Invoke(null, EventArgs.Empty);
            } catch(TaskCanceledException e) {
                Debug.WriteLine($"GameController - calculateFrame Error: Task got Cancled {e.Message}");
            }
        }

        /// <summary>
        /// Runs a series of tasks related to a frame.
        /// </summary>
        /// <param name="deltaTime">The timefactor to consider</param>
        private static void runGameLoopTasks(double deltaTime) {
            List<Task> tasks = new List<Task>();
            // Move opponents
            foreach(IMoveable opponent in Opponents.Values) {
                tasks.Add(Task.Factory.StartNew(() => opponent.Move(deltaTime)));
            }
            //Remove out of bounds bullets, and move the rest
            foreach(IBullet bullet in Bullets.Values) {
                tasks.Add(Task.Factory.StartNew(() => {
                    Point p = UIDispatcher.Invoke(() => { return bullet.BulletShip.Shape.Ray.CenterPoint; });
                    if(bulletOutOfBounds(p)) {
                        BulletAdapter.removeBulletFromCanvas(bullet.ID);
                        Bullets.TryRemove(bullet.ID, out IBullet bulletOut);
                    } else {
                        bullet.BulletShip.Move(deltaTime);
                    }
                }));
            }
            Task.WaitAll(tasks.ToArray());
        }

        /// <summary>
        /// Checks if a bullet is trying to go past the bounds of the arena
        /// </summary>
        private static bool bulletOutOfBounds(Point p) {
            bool result = true;
            int boundInPixels = 6;
            int lowerHeightBound = boundInPixels;
            int lowerWidthBound = boundInPixels;
            int upperWidthBound = ArenaController.ArenaBoundWidth - boundInPixels;
            int upperHeightBound = ArenaController.ArenaBoundHeight - boundInPixels;

            if(p.X > lowerWidthBound && p.X < upperWidthBound && p.Y > lowerHeightBound && p.Y < upperHeightBound) {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Notifies the server that a player has moved
        /// if the player has moved one pixel or degree since last
        /// </summary>
        public static async void notifyMoved() {
            if(ServerTimer.Elapsed.TotalMilliseconds >= 10) { // ish 100 times a second
                ServerTimer.Restart();
                await NetworkController.GameService.PlayerMovedAsync(Player.PlayerShip);
                lastPlayerRay = ((Ray) Player.PlayerShip.Shape.Ray).Clone();
            }
        }

        /// <summary>
        /// Set the number of past calculated frames pr second
        /// to FPS property
        /// </summary>
        static public void calculateFps() {
            try {
                frames++;
                if(fpsTimer.Elapsed.TotalMilliseconds >= 1000) {
                    Fps = frames;
                    frames = 0;
                    fpsTimer.Restart();
                }
            } catch(TaskCanceledException e) {
                Debug.WriteLine($"GameController - CalculateFps Error: Task got Cancled {e.Message}");
            }
        }
    }
}