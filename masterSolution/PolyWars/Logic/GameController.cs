using PolyWars.API;
using PolyWars.Logic.Utility;
using PolyWars.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PolyWars.Logic {
    class GameController {
        private DateTime lastTick;
        private bool isLoaded;
        private DateTime fpsTimer;
        private int frames = 0;
        private Canvas canvas;
        public static List<IShape> Shapes { get; set; }
        public Ticker Ticker { get; private set; }
        public static List<IResource> Resources { get; set; }
        public static List<IShape> Opponents { get; set; }
        public static IPlayer Player { get; set; }
        public int Fps { get; set; }

        /// <summary>
        /// GameController constructor defines all parameter that this class needs to handle
        /// </summary>
        public GameController() {

            isLoaded = false;
            fpsTimer = DateTime.Now;
        }

        /// <summary>
        /// This method prepares the canvas with a color and adds triangle to it with given values
        /// </summary>
        /// <returns>
        /// Returns canvas with background color and triangle
        /// </returns>
        public Canvas prepareGame() {
            Ticker = new Ticker();
            Ticker.TickerEventHandler += calculateFps;
            Ticker.TickerEventHandler += calculateFrame;
            //Ticker.TickerEventHandler += CanvasUpdater;


            Shapes = new List<IShape>();
            Resources = new List<IResource>();
            Opponents = new List<IShape>(); // TODO What type is opponents? IShape? new IOpponents?

            // TODO getOpponents();

            canvas = new Canvas {
                Background = new SolidColorBrush( Colors.Aquamarine ),
            };
            
            createPlayer();
            generateResources( 100 );

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
            int margin = 50;
            int width = ( int ) w.ActualWidth - margin;
            int height = ( int ) w.ActualHeight - margin;
            //int width =  (int)(hej as Grid).ActualWidth - margin;
            //int height = ( int ) ( hej as Grid ).ActualHeight - margin;
            //int width = ( int ) canvas.Width - margin;
            //int height = ( int ) canvas.Height - margin;

            //int resourceCount = Resources.Count;

            for( int i = 0; i < amount; i++ ) {
                Resources.Add( new Resource( new Point( r.Next( margin, width ), r.Next( margin, height - (margin * 2) ) ), r.Next( 0, 360 ), new ShapeSize( 15, 15 ), 5 ) ); //TODO make builder pattern
            }

            //Shapes.AddRange( Resources.GetRange( resourceCount, Resources.Count ) );
            //foreach( IShape shape in Shapes.GetRange(resourceCount, Resources.Count )) {
            //    canvas.Children.Add( shape.Polygon );
            //} //TODO
        }

        private void createPlayer() {
            Player = new Player( new Point( 400, 400 ), 0, Colors.Black, Colors.Gray, new ShapeSize( 50, 50 ), 0, 5, 0, 15 );
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
                Ticker.Start();
            }
        }
        public void endGame() {
            Ticker.Stop();
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

        public void calculateFrame( object sender, TickEventArgs args ) {
            try {
                ThreadController.MainThreadDispatcher.Invoke( () => {
                    ShapeCalculations.moveShape( Player.Shape, args.deltaTime );
                } );
            } catch( TaskCanceledException ) {
                // TODO Should we do something here
            }
        }
        public void calculateFps( object sender, TickEventArgs args ) {
            try {
                lastTick = DateTime.Now;
                if( ( DateTime.Now - fpsTimer ).Ticks >= 10_100_000 ) {
                    Fps = frames + 1;
                    frames = 0;
                    fpsTimer = DateTime.Now;
                } else {
                    frames++;
                }
            } catch( TaskCanceledException ) {
                // TODO Do we need to handle this?
            }
        }
    }
}