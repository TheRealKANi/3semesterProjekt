using PolyWars.API;
using PolyWars.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace PolyWars.Logic {
    class GameController {
        private bool isLoaded;
        private List<IShape> triangles;
        private Renderer renderer;
        public static List<IResource> Resources { get; set; }
        public InputController InputController { get; private set; }

        /// <summary>
        /// GameController constructor defines all parameter that this class needs to handle
        /// </summary>
        public GameController() {

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
            Player t = new Player( new Point( 400, 400 ), 0, Colors.Black, Colors.Gray, new ShapeSize( 50, 50 ), 0, 5, 0, 0.25 );
            InputController.Instance.initInput( t );

            Random r = new Random();
            Resources = new List<IResource>();
            Window w = Application.Current.MainWindow;
            int margin = 50;
            int width = ( int ) w.Width - margin;
            int height = ( int ) w.Height - margin;
            for( int i = 0; i < 200; i++ ) {
                Resources.Add( new Resource( new Point( r.Next( margin, width), r.Next( margin,  height) ), r.Next( 0, 360 ), new ShapeSize( 15, 15 ), 0, 0, 0, 0, 5) );
            }
            triangles.AddRange( Resources );

            triangles.Add( t.Shape );

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