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

    /// <summary>
    /// This class renders the Arena when the game is started 
    /// </summary>
    class GameArena_ViewModel {
        private Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
        public GameController GameController { get; private set; }
        public GameArena_ViewModel() {
            GameController = new GameController();
            ArenaCanvas = GameController.prepareGame();
            GameController.playGame(ArenaCanvas, onCanvasChanged);
        }

        /// <summary>
        /// When the game is started the Arena is associated with a thread
        /// </summary>
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
    }
}
