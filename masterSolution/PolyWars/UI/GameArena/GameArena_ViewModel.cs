using PolyWars.API;
using PolyWars.API.Strategies;
using PolyWars.Logic;
using PolyWars.Model;
using PolyWars.ServerClasses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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

            IPlayer player = createPlayer();
            IEnumerable<IResource> resources = generateResources(200);

            ArenaCanvas = GameController.prepareGame(player, new List<IMoveable>(), resources);
            GameController.CanvasChangedEventHandler += onCanvasChanged;
            GameController.playGame();
            ArenaCanvas.LayoutUpdated += updated;
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
        private ICommand createResources;
        public ICommand CreateResources {
            get {
                if(createResources == null) {
                    createResources = new RelayCommand((o) => { return true; }, (o) => {
                        foreach(IResource r in generateResources(50)) {
                            ArenaCanvas.Children.Add(r.Shape.Polygon);
                        }
                    });
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
        //TODO temp testing stuff below
        private IEnumerable<IResource> generateResources(int amount) {
            Random r = new Random();
            Window w = Application.Current.MainWindow;
            int margin = 50;
            int width = (int) w.ActualWidth - margin;
            int height = (int) w.ActualHeight - margin;

            for(int i = 0; i < amount; i++) {
                IRay ray = new Ray(0, new Point(r.Next(margin, width), r.Next(margin, height - (margin * 2))), r.Next(0, 360));
                IRenderStrategy renderStrategy = new RenderStrategy();
                IRenderable renderable = new Renderable(Colors.Black, Colors.ForestGreen, 1, 15, 15, 4);
                IShape shape = new Shape(0, ray, renderable, renderStrategy);
                IResource resource = new Resource(shape, 5);

                yield return resource;
            }
        }
        private IPlayer createPlayer() {
            IRay ray = new Ray(0, new Point(300, 300), 0);
            IRenderable renderable = new Renderable(Colors.Black, Colors.Gray, 1, 100, 100, 3);
            IShape shape = new Shape(0, ray, renderable, new RenderWithHeaderStrategy());
            IMoveable playerShip = new Moveable(0, 20, 0, 60, shape, new MoveStrategy());
            return new Player("", "", 0, playerShip);
        }
    }
}