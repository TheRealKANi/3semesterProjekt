using PolyWars.API;
using PolyWars.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace PolyWars.Logic
{
    class GameController
    {
        private bool isLoaded;
        private List<IShape> triangles;
        private Renderer renderer;

        public InputController InputController { get; private set; }

        /// <summary>
        /// GameController constructor defines all parameter that this class needs to handle
        /// </summary>
        public GameController( ) {
            
            isLoaded = false;
            triangles = new List<IShape>();

        }

        /// <summary>
        /// This method prepares the canvas with a color and adds triangle to it with given values
        /// </summary>
        /// <returns>
        /// Returns canvas with background color and triangle
        /// </returns>
        public Canvas prepareGame() {
            Canvas canvas = new Canvas {
                Background = new SolidColorBrush( Colors.Aquamarine )
            };

            // TODO Implement a cleaner method to input players
            Triangle t = new Triangle( new Point( 400, 400 ), 0, Colors.Black, Colors.Gray, new ShapeSize( 50, 50 ), 0, 5, 0, 0.25);
            InputController.Instance.initInput( t );
            
            Random r = new Random();
            List<Resource> resources = new List<Resource>();
            for( int i = 0; i < 1000; i++ ) {
                resources.Add( new Resource( new Point( r.Next( 10, 1270 ), r.Next( 10, 710 ) ), r.Next( 0, 360 ), Colors.LawnGreen, Colors.DarkOliveGreen, new ShapeSize( 15, 15 ), 0, 0, 0, 0 ) );
            }
            triangles.AddRange( resources );

            triangles.Add( t );

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
        public void playGame( Canvas arenaCanvas, EventHandler<PropertyChangedEventArgs> eventHandler ) {
            Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
            renderer = new Renderer( arenaCanvas, triangles );
            renderer.CanvasChangedEventHandler += eventHandler;
            // TODO Add inputMovement to Playerand run Renderer here
            //inputController.applyInput( triangles[0] );
            renderer.Start();
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
    }
}