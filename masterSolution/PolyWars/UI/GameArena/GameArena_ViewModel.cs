using PolyWars.Logic;
using PolyWars.Model;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace PolyWars.UI.GameArena {

    /// <summary>
    /// This class renders the Arena when the game is started 
    /// </summary>
    class GameArena_ViewModel : Observable {
        public GameController GameController { get; private set; }
        public GameArena_ViewModel() {
            FpsVisibility = Visibility.Visible;
            GameController = new GameController();
            ArenaCanvas = GameController.prepareGame();
            GameController.Ticker.CanvasChangedEventHandler += onCanvasChanged;
            GameController.playGame();
        }

        /// <summary>
        /// When the game is started the Arena is associated with a thread
        /// </summary>
        public void onCanvasChanged( object Sender, PropertyChangedEventArgs args ) {
            ThreadController.MainThreadDispatcher?.Invoke( () => {
                NotifyPropertyChanged( "ArenaCanvas" );
                NotifyPropertyChanged( "Fps" );
                NotifyPropertyChanged( "PlayerCurrency" );
            } );
        }

        private Visibility fpsVisibility;
        public Visibility FpsVisibility {
            get {
                return fpsVisibility;
            }
            set {
                fpsVisibility = value;
                NotifyPropertyChanged();
            }
        }
        public string Fps {
            get {
                return GameController.Fps.ToString();
            }
        }
        public string PlayerCurrency {
            get {
                return GameController.Player.CurrencyWallet.ToString();
            }
        }
        private ICommand createResources;
        public ICommand CreateResources {
            get {
                if(createResources == null ) {
                    createResources = new RelayCommand( (o) => { return true; }, ( o ) => GameController.generateResources( 50 ) );
                }
                return createResources;
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
    }
}
