using PolyWars.API.Model.Interfaces;
using PolyWars.API.Network.DTO;
using PolyWars.Logic;
using PolyWars.Network;
using PolyWars.ServerClasses;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PolyWars.Adapters {
    class PlayerAdapter {
        public static async Task<ConcurrentDictionary<string, IShape>> OpponentsDTOAdapter() {
            List<PlayerDTO> opponentsDTOs = await NetworkController.GameService.getOpponentsAsync();
            //Console.WriteLine("Got Opponents from server");

            return PlayerDTOtoIShape(opponentsDTOs);
        }

        public static ConcurrentDictionary<string, IShape> PlayerDTOtoIShape(List<PlayerDTO> opponentDTOs) {
            ConcurrentDictionary<string, IShape> opponents = new ConcurrentDictionary<string, IShape>();
            foreach(PlayerDTO opponent in opponentDTOs) {
                if(!opponent.Name.Equals(GameController.Username)) {
                    IRenderable renderable = new Renderable(Colors.Black, Colors.OrangeRed, 1, 25, 25, opponent.Vertices);
                    Shape s = new Shape(opponent.ID, opponent.Ray, renderable, new RenderWithHeaderStrategy());
                    ThreadController.MainThreadDispatcher.Invoke(() => {
                        if(GameController.Opponents != null && GameController.Opponents.ContainsKey(opponent.ID)) {
                            System.Windows.Shapes.Polygon p = GameController.Opponents[opponent.ID].Polygon;
                            ArenaController.ArenaCanvas.Children.Remove(p);
                        }
                    });
                    opponents.TryAdd(s.ID, s);
                    s.Renderer.Render(s.Renderable, s.Ray);
                    ThreadController.MainThreadDispatcher.Invoke(() => {
                        ArenaController.ArenaCanvas.Children.Add(s.Polygon);
                    });

                } else {
                    if(GameController.Player == null) {
                        IRenderable renderable = new Renderable(Colors.Black, Colors.Gray, 1, 25, 25, opponent.Vertices);
                        IShape shape = new Shape(opponent.ID, opponent.Ray, renderable, new RenderWithHeaderStrategy());
                        IMoveable playerShip = new Moveable(0, 20, 0, 180, shape, new MoveStrategy());
                        GameController.Player = new Player(opponent.Name, opponent.ID, opponent.Wallet, playerShip);
                        ThreadController.MainThreadDispatcher.Invoke(() => {
                            ArenaController.ArenaCanvas.Children.Add(GameController.Player.PlayerShip.Shape.Polygon);
                        });
                    } else {
                        GameController.Player.PlayerShip.Mover.Move(GameController.Player.PlayerShip, GameController.DeltaTime(GameController.tickTimer));
                    }
                    //GameController.Player.PlayerShip.Shape = shape;
                }
            }
            //ThreadController.MainThreadDispatcher.Invoke(() => {
            //    GameController.CanvasChangedEventHandler.Invoke(null, EventArgs.Empty);
            //});
            return opponents;
        }
    }
}
