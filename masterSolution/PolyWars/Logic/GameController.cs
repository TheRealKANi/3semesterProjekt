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

        public GameController( ) {
            
            inputController = new InputController();
            isLoaded = false;
            triangles = new ObservableCollection<Triangle>();

        }

        public Canvas prepareGame() {
            Canvas canvas = new Canvas {
                Background = new SolidColorBrush( Colors.Aquamarine )
            };

            // TODO Implement a cleaner method to input players
            Triangle t = new Triangle( new Point( 100, 100 ), 0, Colors.Black, Colors.Gray, new ShapeSize( 50, 50 ) );
            triangles.Add( t );
            return canvas;
        }

        public void playGame( Canvas arenaCanvas, EventHandler<PropertyChangedEventArgs> eventHandler ) {
            Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
            renderer = new Renderer( arenaCanvas, dispatcher, triangles );
            renderer.CanvasChangedEventHandler += eventHandler;
            renderer.Start();
        }

        public bool isPrepared() {
            return isLoaded;
        }
    }
}