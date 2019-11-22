using PolyWars.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using PolyWars.FrameCalculator;
using PolyWars.API;

namespace PolyWars.Logic {

    /// <summary>
    /// Renderer class inherits from Model class Observable
    /// </summary>
    class Renderer : Observable {

        Thread thread;
        private Dispatcher dispatcher;
        private bool stopTickerThread;
        private Task[] moveObjectsTasks;
        private ObservableCollection<Triangle> shapes;
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
        public Renderer( Canvas canvas, Dispatcher dispatcher, ObservableCollection<Triangle> shapes ) {
            Canvas = canvas;
            this.dispatcher = dispatcher;
            this.shapes = shapes;
            stopTickerThread = false;

            //Canvas = canvas;
            moveObjectsTasks = new Task[4];

            foreach( var triangle in shapes ) {
                Canvas.Children.Add( triangle.getShapeAsPolygon( dispatcher ) );
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
            try {
                DateTime Started = DateTime.Now;
                DateTime lastTick = DateTime.Now;
                DateTime fpsTimer = DateTime.Now;
                int frames = 0;


                double DeltaTime( double tickTime ) {
                    return 1d + ( 600_000_000 - tickTime ) / 600_000_000;
                }

                while( !stopTickerThread ) {
                    DateTime tickStart = DateTime.Now;
                    double tickTime = ( tickStart - lastTick ).Ticks;

                    void MoveObjects( int start, int range ) {
                        for( int i = start; i < range; i++ ) {
                            Triangle triangle = shapes[i];
                            double rotationPerTick = triangle.RPM * DeltaTime( tickTime );
                            double xPerTick = triangle.HorizontialSpeed * DeltaTime( tickTime );
                            double yPerTick = triangle.VerticalSpeed * DeltaTime( tickTime );
                            //Point newCenterPoint = new Point( xPerTick, yPerTick );
                            //triangle.CenterPoint = newCenterPoint;
                            //triangle.RPM = rotationPerTick;
                            MoveShapes.move( triangle, dispatcher );
                            //triangle.Move( xPerTick, yPerTick, rotationPerTick );
                        }
                    }

                    if( shapes.Count > 3 ) {
                        int divisible4 = shapes.Count - shapes.Count % 4;
                        int[] ranges = new int[4];
                        ranges[0] = divisible4 / 4;
                        ranges[1] = divisible4 / 4 + ranges[0];
                        ranges[2] = divisible4 / 4 + ranges[1];
                        ranges[3] = divisible4 / 4 + ranges[2];

                        for( int i = 0; i < shapes.Count % 4; i++ ) {
                            ranges[i] += 1;
                        }
                        moveObjectsTasks[0] = Task.Run( () => MoveObjects( 0, ranges[0] ) );
                        moveObjectsTasks[1] = Task.Run( () => MoveObjects( ranges[0], ranges[1] ) );
                        moveObjectsTasks[2] = Task.Run( () => MoveObjects( ranges[1], ranges[2] ) );
                        moveObjectsTasks[3] = Task.Run( () => MoveObjects( ranges[2], ranges[3] ) );

                        Task.WaitAll( moveObjectsTasks );
                    } else {
                        MoveObjects( 0, shapes.Count-1 );
                    }


                    int s;
                    while( ( DateTime.Now.Ticks - lastTick.Ticks ) <= ( 10_000_000d / 60 ) ) {
                        s = ( int ) ( ( 1d / 60 - ( double ) ( DateTime.Now - lastTick ).Ticks ) / 2 );

                        if( s > 40000 ) {
                            Thread.Sleep( s >= 0 ? s : 0 );
                        } else if( s > 0 ) {
                            Thread.Sleep( 1 );
                        }
                    }

                    dispatcher.Invoke(() => CanvasChangedEventHandler?.Invoke( this, new PropertyChangedEventArgs( "ArenaCanvas" ) ));
                    lastTick = DateTime.Now;
                    if( ( DateTime.Now - fpsTimer ).Ticks >= 10_100_000 ) {
                        Fps = frames + 1;
                        frames = 0;
                        fpsTimer = DateTime.Now;
                    } else {
                        frames++;
                    }
                }
        } catch(Exception ) {

                stopTickerThread = true;
            }
}
    }
}
