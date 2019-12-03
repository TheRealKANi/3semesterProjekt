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

namespace PolyWars.Logic {
    class GameController {

        private static bool isPrepaired;
        private static int frames = 0;
        private static Stopwatch fpsTimer;
        public static Ticker Ticker { get; private set; }
        public static IPlayer Player { get; set; }
        public static string Username { get; set; }
        public static string UserID { get; set; }
        public static IEnumerable<IShape> Immovables { get; set; }
        public static List<IResource> Resources { get; set; }
        public static int Fps { get; set; }
        public static Stopwatch tickTimer { get; private set; }
        private static Stopwatch ServerTimer { get; set; }
        public static double ArenaWidth { get; set; }
        public static double ArenaHeight { get; set; }

        private const decimal baselineFps = 1000m / 60; // miliseconds per frame at 60 fps 
        static public EventHandler<EventArgs> CanvasChangedEventHandler;
        private static bool serverResponded;


        /// <summary>
        /// Default constructor of GameController Class
        /// </summary>
        public GameController() {
            serverResponded = true;
            isPrepaired = false;
            fpsTimer = new Stopwatch();
            fpsTimer.Reset();
            ServerTimer = new Stopwatch();
            ServerTimer.Start();
        }

        public async Task prepareGame() {
            Ticker = new Ticker();
            tickTimer = new Stopwatch();
            
            ArenaController.generateCanvas();
            Player = createBlankPlayer();
            Resources = await Adapters.ResourceAdapter.ResourceDTOAdapter() ?? new List<IResource>();
            Immovables = await Adapters.PlayerAdapter.OpponentsDTOAdapter() ?? new List<IShape>();
            Immovables = new List<IShape>();

            isLoaded = true;
        }

        public void playGame() {
            if(isPrepaired) {
                fpsTimer.Start();
                Ticker.Start();
            }
        }

        public void endGame() {
            Ticker.Stop();
            fpsTimer.Stop();
        }

        private IPlayer createBlankPlayer() {
            IRay ray = new Ray(UserID, new Point(300, 300), 0);
            IRenderable renderable = new Renderable(Colors.Black, Colors.DimGray, 1, 25, 25, 3);
            IShape shape = new Shape(UserID, ray, renderable, new RenderWithHeaderStrategy());
            IMoveable playerShip = new Moveable(0, 20, 0, 180, shape, new MoveStrategy());
            return new Player(Username, UserID, 0, playerShip);
        }

        private static decimal DeltaTime(Stopwatch _tickTimer) {
            return (decimal) _tickTimer.Elapsed.TotalMilliseconds / baselineFps;
        }

        static public void calculateFrame() {
            try {
                ThreadController.MainThreadDispatcher.Invoke(() => {
                    Player.PlayerShip.Move(DeltaTime(tickTimer));
                    tickTimer.Stop();
                    CanvasChangedEventHandler?.Invoke(null, EventArgs.Empty);
                });
            } catch(TaskCanceledException) {
                // TODO Should we do something here
            }
            try {
                // Sends player iray to server
                Task.Run(() => notifyMoved());
            } catch(Exception e) {
                Debug.WriteLine("Error:" + e.Message);
            }
        }
        public static async void notifyMoved() {
            if(serverResponded && ServerTimer.Elapsed.TotalMilliseconds >= (950/60d)) {
                ServerTimer.Restart();
                serverResponded = false;
                serverResponded = await NetworkController.GameService.PlayerMovedAsync(Player.PlayerShip.Shape.Ray);
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