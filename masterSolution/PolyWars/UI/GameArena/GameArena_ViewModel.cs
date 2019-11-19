using PolyWars.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace PolyWars.UI.GameArena {
    class GameArena_ViewModel {
        private Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
        private GameController gameController;
        public GameArena_ViewModel() {

            gameController = new GameController();
            ArenaCanvas = gameController.prepareGame();
            gameController.playGame(ArenaCanvas, onCanvasChanged);
        }

        public void onCanvasChanged( object Sender, PropertyChangedEventArgs args ) {
            //NotifyPropertyChanged(args.PropertyName);
            if( args.PropertyName.Equals( "ArenaCanvas" ) ) {
                if( Sender is Renderer r ) {
                    dispatcher.Invoke( () => NotifyPropertyChanged("ArenaCanvas"));
                }
            }
        }

        private Canvas arenaCanvas;
        public Canvas ArenaCanvas {
            get {
                return arenaCanvas;
            }
            set {
                arenaCanvas = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //public void OnCanvasChanged( object sender, PropertyChangedEventArgs e ) {
        //    if( e.PropertyName.Equals( "ArenaCanvas" ) ) {
        //        if( sender is Renderer r ) {
        //            dispatcher.Invoke( () => ArenaCanvas = r.Canvas );
        //        }
        //    }
        //}
    }
}
