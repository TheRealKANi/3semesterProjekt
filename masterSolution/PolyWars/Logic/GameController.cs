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

namespace PolyWars.Logic {
    class GameController {
        private bool isLoaded;
        static private int frames = 0;
        private Canvas canvas;
        public static List<IShape> Shapes { get; set; }
        public Ticker Ticker { get; private set; }
        public static List<IResource> Resources { get; set; }
        public static List<IShape> Opponents { get; set; }
        public static IPlayer Player { get; set; }
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
        public Canvas prepareGame() {
            Ticker = new Ticker();
            tickTimer = new Stopwatch();

            Shapes = new List<IShape>();
            Resources = new List<IResource>();
            Opponents = new List<IShape>(); // TODO What type is opponents? IShape? new IOpponents?

            // TODO getOpponents();

            canvas = new Canvas {
                Background = new SolidColorBrush( Colors.Aquamarine ),
            };
            
            createPlayer();
            generateResources( 500 );

            // TODO DEBUG - Init Frame Timer
            Utility.FrameDebugTimer.initTimers();

            Shapes.AddRange( Resources );
            Shapes.AddRange( Opponents );
            Shapes.Add( Player.Shape );


            foreach( IShape shape in Shapes ) {
                canvas.Children.Add( shape.Polygon );
            }

            isLoaded = true;

            return canvas;
        }

        public void generateResources( int amount ) {
            Random r = new Random();
            Window w = Application.Current.MainWindow;
            object hej = Application.Current.MainWindow.Content;
            int margin = 50;
            int width = ( int ) w.ActualWidth - margin;
            int height = ( int ) w.ActualHeight - margin;

            for( int i = 0; i < amount; i++ ) {
                Resources.Add( new Resource( new Point( r.Next( margin, width ), r.Next( margin, height - (margin * 2) ) ), r.Next( 0, 360 ), new ShapeSize( 15, 15 ), 5 ) ); //TODO make builder pattern
            }
        }

        private void createPlayer() {
            Player = new Player( new Point( 400, 400 ), 0, Colors.Black, Colors.Gray, new ShapeSize( 50, 50 ), 0, 50, 0, 180 );
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
                    ShapeCalculations.moveShape( Player.Shape, DeltaTime( tickTimer ));
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