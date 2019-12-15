using PolyWars.Client.Logic;
using PolyWars.Client.Model;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PolyWars.UI.GameArena {

    /// <summary>
    /// This class renders the Arena when the game is started 
    /// </summary>
    class GameArena_ViewModel : Observable {
        public GameController GameController { get; private set; }
        public double ArenaHeight { get; set; }
        public double ArenaWidth { get; set; }
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
                if(GameController.Player != null) {
                    return GameController.Player.Wallet.ToString();
                }
                return "-1";
            }
        }

        public string PlayerHealth {
            get {
                if(GameController.Player != null) {
                    return GameController.Player.Health.ToString();
                }
                return "-1";
            }
        }

        private Canvas arenaCanvas;
        public Canvas ArenaCanvas {
            get {
                return arenaCanvas;
            }
            set {
                arenaCanvas = value;

                ArenaCanvas.Loaded += OnCanvasLoaded;
                GameController.CanvasChangedEventHandler += onCanvasChanged;
                ArenaCanvas.LayoutUpdated += updated;
                ArenaCanvas.LayoutUpdated += GameController.Ticker.onFrameDisplayed;

                NotifyPropertyChanged();
            }
        }

        private void OnCanvasLoaded(object sender, RoutedEventArgs e) {
            if(sender is Canvas canvas) {
                GameController.ArenaHeight = canvas.ActualHeight;
                GameController.ArenaWidth = canvas.ActualWidth;
                ArenaCanvas.UpdateLayout();
            }
        }

        public GameArena_ViewModel() {
            FpsVisibility = Visibility.Visible;
            ArenaCanvas = ArenaController.ArenaCanvas;
        }

        private void updated(object sender, EventArgs e) {
            GameController.calculateFps();
        }

        /// <summary>
        /// When the game is started the Arena is associated with a thread
        /// </summary>
        public void onCanvasChanged(object Sender, EventArgs args) {
            UIDispatcher.Invoke(() => {
                NotifyPropertyChanged("ArenaCanvas");
                NotifyPropertyChanged("Fps");
                NotifyPropertyChanged("PlayerCurrency");
                NotifyPropertyChanged("PlayerHealth");
            });
        }
    }
}