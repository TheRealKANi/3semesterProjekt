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

        private static bool isLoaded;
        private static int frames = 0;
        private static Stopwatch fpsTimer;
        public static Ticker Ticker { get; private set; }
        public static IPlayer Player { get; private set; }
        public static IEnumerable<IShape> Immovables { get; private set; }
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
            isLoaded = false;
            fpsTimer = new Stopwatch();
            fpsTimer.Reset();
            ServerTimer = new Stopwatch();
            ServerTimer.Start();
        }

        public void prepareGame() {
            Ticker = new Ticker();
            tickTimer = new Stopwatch();

            ArenaController.generateCanvas();
            Immovables = new List<IShape>();
            Resources =  new List<IResource>();

            

            NetworkController.GameService.getResourcesAsync().Wait();

            Task<List<IResource>> resourceTask = Adapters.ResourceAdapter.ResourceDTOAdapter();
            //Task<List<IResource>> OppnentTask = Adapters.ResourceAdapter.ResourceDTOAdapter();
            
            resourceTask.Start();
            //tasks[1] = opponents
            
            Task.WaitAll(resourceTask);
            Resources = resourceTask.Result;

            FrameDebugTimer.initTimers();
            Player = createPlayer();
            // TODO getOpponents();

            // TODO DEBUG - Init Frame Timer
            isLoaded = true;
        }

        public void playGame() {
            if(isPrepared()) {
                fpsTimer.Start();
                Ticker.Start();
            }
        }

        public void endGame() {
            Ticker.Stop();
            fpsTimer.Stop();
        }

        //public static void generateResources(int amount) {
        //    Random r = new Random();
        //    int margin = 15;
        //    int width = (int) ArenaWidth - margin;
        //    int height = (int) ArenaHeight - margin;
        //    Resources = new List<IResource>();
        //    for(int i = 0; i < amount; i++) {
        //        IRay ray = new Ray(0, new Point(r.Next(margin, width), r.Next(margin, height)), r.Next(0, 360));
        //        IRenderStrategy renderStrategy = new RenderStrategy();
        //        IRenderable renderable = new Renderable(Colors.Black, Colors.ForestGreen, 1, 15, 15, 4);
        //        IShape shape = new Shape(0, ray, renderable, renderStrategy);
        //        IResource resource = new Resource(0, shape, 5);
        //        Resources.Add(resource);
        //    }
        //}

        private IPlayer createPlayer() {
            IRay ray = new Ray("0", new Point(300, 300), 0);
            IRenderable renderable = new Renderable(Colors.Black, Colors.Gray, 1, 25, 25, 7);
            IShape shape = new Shape("0", ray, renderable, new RenderWithHeaderStrategy());
            IMoveable playerShip = new Moveable(0, 20, 0, 180, shape, new MoveStrategy());
            playerShip.Shape.Polygon.Fill = new SolidColorBrush(Colors.DimGray);
            return new Player("", "", 0, playerShip);
        }

        public bool isPrepared() {
            return isLoaded;
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