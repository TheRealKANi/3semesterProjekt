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
        private InputController inputController;
        private bool isLoaded;
        private ObservableCollection<Triangle> triangles;
        private Renderer renderer;

        /// <summary>
        /// GameController constructor defines all parameter that this class needs to handle
        /// </summary>
        public GameController( ) {
            
            isLoaded = false;
            triangles = new ObservableCollection<Triangle>();
            inputController = new InputController();
            inputController.initInput();

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
            Triangle t = new Triangle( new Point( 100, 100 ), 0, Colors.Black, Colors.Gray, new ShapeSize( 50, 50 ) );
            //Triangle t1 = new Triangle( new Point( 150, 100 ), 45, Colors.Black, Colors.White, new ShapeSize( 50, 50 ) );
            //Triangle t2 = new Triangle( new Point( 200, 100 ), 90, Colors.Black, Colors.MediumOrchid, new ShapeSize( 50, 50 ) );
            //Triangle t3 = new Triangle( new Point( 250, 100 ), 135, Colors.Black, Colors.Pink, new ShapeSize( 50, 50 ) );

            //Triangle t4 = new Triangle( new Point( 100, 200 ), 180, Colors.Black, Colors.Red, new ShapeSize( 50, 50 ) );
            //Triangle t5 = new Triangle( new Point( 150, 200 ), 225, Colors.Black, Colors.Blue, new ShapeSize( 50, 50 ) );
            //Triangle t6 = new Triangle( new Point( 200, 200 ), 270, Colors.Black, Colors.Silver, new ShapeSize( 50, 50 ) );
            //Triangle t7 = new Triangle( new Point( 250, 200 ), 320, Colors.Black, Colors.Gold, new ShapeSize( 50, 50 ) );
            //Triangle t8 = new Triangle( new Point( 100, 300 ), 360, Colors.Black, Colors.Gold, new ShapeSize( 50, 50 ) );
            triangles.Add( t );
            //triangles.Add( t1 );
            //triangles.Add( t2 );
            //triangles.Add( t3 );
            //triangles.Add( t4 );
            //triangles.Add( t5 );
            //triangles.Add( t6 );
            //triangles.Add( t7 );
            //triangles.Add( t8 );
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
            renderer = new Renderer( arenaCanvas, dispatcher, triangles );
            renderer.CanvasChangedEventHandler += eventHandler;
            // TODO Add input and the player to Rendere here
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