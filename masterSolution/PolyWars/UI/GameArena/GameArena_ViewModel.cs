using PolyWars.API;
using PolyWars.API.Strategies;
using PolyWars.Logic;
using PolyWars.Model;
using PolyWars.ServerClasses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
        private IPlayer player;
        private IEnumerable<IResource> resources;
        private IEnumerable<IShape> immovables;

        public GameController GameController { get; private set; }
        public GameArena_ViewModel() {
            ArenaCanvas = new Canvas {
                Background = new SolidColorBrush(Colors.Aquamarine),
            };

            FpsVisibility = Visibility.Visible;
            GameController = new GameController();

            player = createPlayer();

            immovables = new List<IShape>();
            resources = new List<IResource>();


            GameController.prepareGame(player, immovables, resources);


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
                
                ArenaCanvas.Loaded += OnCanvasLoaded;
                GameController.CanvasChangedEventHandler += onCanvasChanged;
                ArenaCanvas.LayoutUpdated += updated;

                NotifyPropertyChanged();
            }
        }
        public double ArenaHeight { get; set; }
        public double ArenaWidth { get; set; }

        private void OnCanvasLoaded(object sender, RoutedEventArgs e) {
            if(sender is Canvas canvas) {
                ArenaHeight = canvas.ActualHeight;
                ArenaWidth = canvas.ActualWidth;
                
                resources = generateResources(50);
                ArenaController.fillArena(ArenaCanvas, player, immovables, resources);
                ArenaCanvas.UpdateLayout();
                GameController.playGame();
            }
        }

        //TODO temp testing stuff below
        private IEnumerable<IResource> generateResources(int amount) {
            Random r = new Random();
            int margin = 15;
            int width = (int) ArenaWidth - margin;
            int height = (int) ArenaHeight - margin;

            for(int i = 0; i < amount; i++) {
                //IRay ray = new Ray(0, new Point(r.Next(margin, width), r.Next(margin, height - (margin * 2))), r.Next(0, 360));
                IRay ray = new Ray(0, new Point(r.Next(margin, width), r.Next(margin, height)), r.Next(0, 360));
                IRenderStrategy renderStrategy = new RenderStrategy();
                IRenderable renderable = new Renderable(Colors.Black, Colors.ForestGreen, 1, 15, 15, 4);
                IShape shape = new Shape(0, ray, renderable, renderStrategy);
                IResource resource = new Resource(shape, 5);

                yield return resource;
            }
        }
        private IPlayer createPlayer() {
            IRay ray = new Ray(0, new Point(300, 300), 0);
            IRenderable renderable = new Renderable(Colors.Black, Colors.Gray, 1, 50, 50, 3);
            IShape shape = new Shape(0, ray, renderable, new RenderWithHeaderStrategy());
            IMoveable playerShip = new Moveable(0, 20, 0, 60, shape, new MoveStrategy());
            return new Player("", "", 0, playerShip);
        }
    }
}