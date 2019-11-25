using PolyWars.API;
using PolyWars.FrameCalculator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PolyWars.Logic {

    /// <summary>
    /// Renderer class inherits from Model class Observable
    /// </summary>
    class Renderer {

        Thread thread;
        private bool stopTickerThread;
        private Task[] moveObjectsTasks;
        private List<IShape> shapes;
        public EventHandler<PropertyChangedEventArgs> CanvasChangedEventHandler;


        public Canvas Canvas { get; set; }

        public int Fps { get; set; }

        /// <summary>
        /// Public constructor for class renderer 
        /// </summary>
        /// <param name="canvas">
        /// Takes a Canvas as parameter
        /// </param>
        /// <param name="dispatcher">
        /// Renderer gets a dispatcher 
        /// </param>
        /// <param name="shapes">
        /// Takes an ObservableCollection as parameter for dynamic data collection when items is added and removed
        /// </param>
        public Renderer( Canvas canvas, List<IShape> shapes ) {
            Canvas = canvas;
            this.shapes = shapes;
            stopTickerThread = false;

            //Canvas = canvas;
            moveObjectsTasks = new Task[4];

            foreach( IShape triangle in shapes ) {
                Canvas.Children.Add( triangle.Polygon );
            }

            //A new thread is created 
            thread = new Thread( Ticker );
        }

        /// <summary>
        /// Starts the thread Ticker
        /// </summary>
        public void Start() {
            stopTickerThread = false;
            thread.Start();
        }

        /// <summary>
        /// Stops the thread Ticker
        /// </summary>
        public void Stop() {
            stopTickerThread = true;
        }

        /// <summary>
        /// The ticker method is used to give the game a balance between its pace and frames per secod 
        /// This is possible by using DeltaTime
        /// </summary>
        private void Ticker() {
            // TODO Remove/Refactor try catch block
            DateTime Started = DateTime.Now;
            DateTime lastTick = DateTime.Now;
            DateTime fpsTimer = DateTime.Now;
            int frames = 0;


            double DeltaTime( double tickTime ) {
                return 1d + ( ( 600_000_000 - tickTime ) / 600_000_000 );
            }

            while( !stopTickerThread ) {
                DateTime tickStart = DateTime.Now;
                double tickTime = ( tickStart - lastTick ).Ticks;

                InputController.Instance.queryInput();

                try {
                    ThreadController.MainThreadDispatcher.Invoke( () => {
                        foreach( IShape shape in shapes ) {
                            if( shape.GetType().Name.Equals( "Triangle" ) ) {
                                MoveShapes.move( shape, DeltaTime( tickTime ) );
                                MoveShapes.collisionDetection( Canvas, shapes.ElementAt( shapes.Count() - 1 ) );
                                //Debug.WriteLine( "Shape Type: '" + shape.GetType().Name.ToString() + "'" );
                            }
                        }
                    } );
                } catch( TaskCanceledException ) {
                    // TODO Should we do something here
                }
                

                int s;
                while( ( DateTime.Now.Ticks - lastTick.Ticks ) <= ( 10_000_000d / 60 ) ) {
                    s = ( int ) ( ( ( 1d / 60 ) - ( double ) ( DateTime.Now - lastTick ).Ticks ) / 2 );

                    if( s > 40000 ) {
                        Thread.Sleep( s >= 0 ? s : 0 );
                    } else if( s > 0 ) {
                        Thread.Sleep( 1 );
                    }
                }
                try {
                    ThreadController.MainThreadDispatcher.Invoke( () => CanvasChangedEventHandler?.Invoke( this, new PropertyChangedEventArgs( "ArenaCanvas" ) ) );
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
}