using PolyWars.Logic;
using PolyWars.Model;
using System;
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
                return GameController.Player.Wallet.ToString();
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
                //GameController.generateResources(500);
                ArenaController.fillArena();

                ArenaCanvas.UpdateLayout();
                GameController.playGame();
            }
        }

        public GameArena_ViewModel() {
            FpsVisibility = Visibility.Visible;
            GameController = new GameController();
            GameController.prepareGame();
            ArenaCanvas = ArenaController.ArenaCanvas;
        }

        private void updated(object sender, EventArgs e) {
            GameController.calculateFps();
        }

        /// <summary>
        /// When the game is started the Arena is associated with a thread
        /// </summary>
        public void onCanvasChanged(object Sender, EventArgs args) {
            ThreadController.MainThreadDispatcher?.Invoke(() => {
                NotifyPropertyChanged("ArenaCanvas");
                NotifyPropertyChanged("Fps");
                NotifyPropertyChanged("PlayerCurrency");
            });
        }
    }
}