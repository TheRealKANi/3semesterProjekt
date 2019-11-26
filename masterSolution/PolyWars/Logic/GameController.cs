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
        private DateTime lastTick;
        private bool isLoaded;
        private DateTime fpsTimer;
        private int frames = 0;

        public static List<IShape> Shapes { get; set; }
        public Ticker Ticker { get; private set; }
        public static List<IResource> Resources { get; set; }
        public static List<IShape> Opponents { get; set; }
        public static IPlayer Player { get; set; }
        private int fps;
        public int Fps { get { return fps; } private set { fps = value; Debug.WriteLine( fps ); } } //TODO proper fps display on ui

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

            createPlayer();
            generateResources( 200 );
            // TODO getOpponents();



            Shapes.AddRange( Resources );
            Shapes.AddRange( Opponents );
            Shapes.Add( Player.Shape );
            
            Canvas canvas = new Canvas {
                Background = new SolidColorBrush( Colors.Aquamarine ),
            };

            foreach( IShape shape in Shapes ) {
                canvas.Children.Add( shape.Polygon );
            }

            isLoaded = true;

            return canvas;
        }

        private void generateResources( int amount ) {
            Random r = new Random();
            Window w = Application.Current.MainWindow;
            int margin = 50;
            int width = ( int ) w.Width - margin;
            int height = ( int ) w.Height - margin;

            for( int i = 0; i < amount; i++ ) {
                Resources.Add( new Resource( new Point( r.Next( margin, width ), r.Next( margin, height ) ), r.Next( 0, 360 ), new ShapeSize( 15, 15 ), 5 ) ); //TODO make builder pattern
            }
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
                    foreach( IShape shape in Shapes ) {
                        if( shape.GetType().Name.Equals( Player.PlayerShape ) ) {
                            ShapeCalculations.moveShape( shape, args.deltaTime );
                            CollisionDetection.resourceCollisionDetection();
                        }
                    }
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
        //public void canvasupdater( object sender, tickeventargs args ) {
        //    try {
        //        threadcontroller.mainthreaddispatcher.invoke( () => canvaschangedeventhandler?.invoke( this, new propertychangedeventargs( "arenacanvas" ) ) );
        //    } catch( taskcanceledexception ) {
        //        //todo ?!
        //    }
        //}
    }
}