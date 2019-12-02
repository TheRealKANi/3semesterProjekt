using PolyWars.Api;
using PolyWars.API;
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

        static private bool isLoaded;
        static private int frames = 0;
        static private Stopwatch fpsTimer;
        static public Ticker Ticker { get; private set; }
        static public IPlayer Player { get; private set; }
        static public IEnumerable<IShape> Immovables { get; private set; }
        static public List<IResource> Resources { get; private set; }
        static public int Fps { get; set; }
        static public Stopwatch tickTimer { get; private set; }
        static public double ArenaWidth { get; set; }
        static public double ArenaHeight { get; set; }

        private const decimal baselineFps = 1000m / 60; // miliseconds per frame at 60 fps 
        static public EventHandler<EventArgs> CanvasChangedEventHandler;


        /// <summary>
        /// Default constructor of GameController Class
        /// </summary>
        public GameController() {

            isLoaded = false;
            fpsTimer = new Stopwatch();
            fpsTimer.Reset();
        }

        public void prepareGame() {
            Ticker = new Ticker();
            tickTimer = new Stopwatch();

            ArenaController.generateCanvas();
            Immovables = new List<IShape>();

            Player = createPlayer();
            // TODO getOpponents();

            // TODO DEBUG - Init Frame Timer
            FrameDebugTimer.initTimers();
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

        public static void generateResources(int amount) {
            Random r = new Random();
            int margin = 15;
            int width = (int) ArenaWidth - margin;
            int height = (int) ArenaHeight - margin;
            Resources = new List<IResource>();
            for(int i = 0; i < amount; i++) {
                IRay ray = new Ray(0, new Point(r.Next(margin, width), r.Next(margin, height)), r.Next(0, 360));
                IRenderStrategy renderStrategy = new RenderStrategy();
                IRenderable renderable = new Renderable(Colors.Black, Colors.ForestGreen, 1, 15, 15, 4);
                IShape shape = new Shape(0, ray, renderable, renderStrategy);
                IResource resource = new Resource(shape, 5);
                Resources.Add(resource);
            }
        }

        private IPlayer createPlayer() {
            IRay ray = new Ray(0, new Point(300, 300), 0);
            IRenderable renderable = new Renderable(Colors.Black, Colors.Gray, 1, 25, 25, 3);
            IShape shape = new Shape(0, ray, renderable, new RenderWithHeaderStrategy());
            IMoveable playerShip = new Moveable(0, 20, 0, 60, shape, new MoveStrategy());
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
                NetworkController.GameService.PlayerMovedAsync(Player.PlayerShip.Shape.Ray);
            } catch(Exception e) {
                Debug.WriteLine("Error:" + e.Message);
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