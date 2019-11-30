using PolyWars.API;
using PolyWars.Logic.Utility;
using PolyWars.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Linq;
using PolyWars.ServerClasses;
using PolyWars.API.Strategies;

namespace PolyWars.Logic {
    class GameController {
        private bool isLoaded;
        static private int frames = 0;
        private Canvas canvas;
        public Ticker Ticker { get; private set; }
        public static IPlayer Player { get; private set; }
        public IEnumerable<IMoveable> Moveables { get; private set; }
        public IEnumerable<IResource> Resources { get; private set; }
        static public int Fps { get; set; }
        static private Stopwatch fpsTimer;
        public static Stopwatch tickTimer { get; private set; }
        private const decimal baselineFps = 1000m / 60; // miliseconds per frame at 60 fps 

        static public EventHandler<EventArgs> CanvasChangedEventHandler;


        /// <summary>
        /// GameController constructor defines all parameter that this class needs to handle
        /// </summary>
        public GameController() {

            isLoaded = false;
            fpsTimer = new Stopwatch();
            fpsTimer.Reset();
        }

        /// <summary>
        /// This method prepares the canvas with a color and adds triangle to it with given values
        /// </summary>
        /// <returns>
        /// Returns canvas with background color and triangle
        /// </returns>
        public Canvas prepareGame(IPlayer player, IEnumerable<IMoveable> moveables, IEnumerable<IResource> resources) {
            Ticker = new Ticker();
            tickTimer = new Stopwatch();

            Player = player;
            Moveables = moveables;
            Resources = resources;

            canvas = new Canvas {
                Background = new SolidColorBrush( Colors.Aquamarine ),
            };

            // TODO getOpponents();

            // TODO DEBUG - Init Frame Timer
            Utility.FrameDebugTimer.initTimers();

            canvas.Children.Add(player.PlayerShip.Shape.Polygon);

            foreach( IShape shape in moveables.Select(x => x.Shape ).Concat(Resources.Select(x => x.Shape))) {
                canvas.Children.Add( shape.Polygon );
            }

            isLoaded = true;

            return canvas;
        }

        

        /// <summary>
        /// When PlayGame executes, it associates with a thread by using a dispatcher
        /// and renders a canvas
        /// </summary>
        /// <param name="arenaCanvas">
        /// Specified canvas that shapes are rendered on to
        /// </param>
        /// <param name="eventHandler">
        /// ????
        /// </param>
        public void playGame() {
            if( isPrepared() ) {
                fpsTimer.Start();
                Ticker.Start();
            }
        }
        public void endGame() {
            Ticker.Stop();
            fpsTimer.Stop();
        }

        /// <summary>
        /// This method is run to check if the game runs 
        /// </summary>
        /// <returns>
        /// Loads the game if not already running
        /// </returns>

        public bool isPrepared() {
            return isLoaded;
        }
        private static decimal DeltaTime( Stopwatch _tickTimer ) {
            return (decimal)_tickTimer.Elapsed.TotalMilliseconds / baselineFps;
        }
        static public void calculateFrame( ) {
            try {
                ThreadController.MainThreadDispatcher.Invoke( () => {
                    Player.PlayerShip.Move(DeltaTime(tickTimer));
                    tickTimer.Stop();
                    CanvasChangedEventHandler?.Invoke( null, EventArgs.Empty );
                } );
            } catch( TaskCanceledException ) {
                // TODO Should we do something here
            }
        }
        static public void calculateFps() {
            try {
                frames++;
                if( fpsTimer.Elapsed.TotalMilliseconds >= 1000) {
                    Fps = frames;
                    frames = 0;
                    fpsTimer.Restart();
                    
                }
            } catch( TaskCanceledException ) {
                // TODO Do we need to handle this?
            }
        }
    }
}