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

namespace PolyWars.Logic {
    class GameController {

        private static bool isPrepared;
        private static int frames = 0;
        private static Stopwatch fpsTimer;
        private static Ray lastRay;
        public static Ticker Ticker { get; private set; }
        public static IPlayer Player { get; set; }
        public static string Username { get; set; }
        public static string UserID { get; set; }
        public static ConcurrentDictionary<string, IShape> Opponents { get; set; }
        public static ConcurrentDictionary<string, IResource> Resources { get; set; }
        public ConcurrentDictionary<string, IBullet> Bullets { get; private set; }
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
        static GameController() {
            serverResponded = true;
            isPrepared = false;
            fpsTimer = new Stopwatch();
            fpsTimer.Reset();
            Ticker = new Ticker();
            tickTimer = new Stopwatch();
            ServerTimer = new Stopwatch();
            ServerTimer.Start();
        }

        public async Task prepareGame() {
            ArenaController.generateCanvas();
            Opponents = await Adapters.PlayerAdapter.OpponentsDTOAdapter() ?? new ConcurrentDictionary<string, IShape>();
            Resources = await Adapters.ResourceAdapter.ResourceDTOAdapter() ?? new ConcurrentDictionary<string, IResource>();
            Bullets = await Adapters.BulletAdapter.BulletDTOAdapter() ?? new ConcurrentDictionary<string, IBullet>();
            isPrepared = true;
        }

        public async Task playGame() {
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
                ThreadController.MainThreadDispatcher.Invoke(() => {
                    Player.PlayerShip.Move(DeltaTime(tickTimer)); 
                    if(lastRay == null || !lastRay.IsEqual(Player.PlayerShip.Shape.Ray)) {
                        Task.Run(() => notifyMoved());
                    }
                    tickTimer.Stop();
                    CanvasChangedEventHandler?.Invoke(null, EventArgs.Empty);
                });
            } catch(TaskCanceledException) {
                // TODO Should we do something here
            }
        }
        public static async void notifyMoved() {
            if(serverResponded && ServerTimer.Elapsed.TotalMilliseconds >= 100 ) { // ish 10 times a second
                ServerTimer.Restart();
                serverResponded = false;
                lastRay = ((Ray)Player.PlayerShip.Shape.Ray).Clone();
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