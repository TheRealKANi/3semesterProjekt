using PolyWars.API.Model.Interfaces;
using PolyWars.API.Network.DTO;
using PolyWars.Logic;
using PolyWars.Network;
using PolyWars.ServerClasses;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PolyWars.Adapters {
    class PlayerAdapter {
        public static async Task<ConcurrentDictionary<string, IShape>> OpponentsDTOAdapter() {
            List<PlayerDTO> opponentsDTOs = await NetworkController.GameService.getOpponentsAsync();
            Debug.WriteLine("Got Opponents from server");

            return PlayerDTOtoIShape(opponentsDTOs);
        }

        public static ConcurrentDictionary<string, IShape> PlayerDTOtoIShape(List<PlayerDTO> opponentDTOs) {
            ConcurrentDictionary<string, IShape> opponents = new ConcurrentDictionary<string, IShape>(); // TODO Use this as conccurrency issue?
            foreach(PlayerDTO opponent in opponentDTOs) {
                if(!opponent.Name.Equals(GameController.Username)) {
                    IRenderable renderable = new Renderable(Colors.Black, Colors.HotPink, 1, 25, 25, opponent.Vertices);
                    Shape shape = new Shape(opponent.Name, opponent.Ray, renderable, new RenderWithHeaderStrategy());
                    shape.Renderer.Render(shape.Renderable, shape.Ray);
                    if(GameController.Opponents.TryAdd(opponent.Name, shape)) {
                        addOpponentToCanvas(shape);
                    }
                } else {
                    if(GameController.Player == null) {
                        IRenderable renderable = new Renderable(Colors.Black, Colors.Gray, 1, 25, 25, opponent.Vertices);
                        IShape shape = new Shape(opponent.Name, opponent.Ray, renderable, new RenderWithHeaderStrategy());
                        IMoveable playerShip = new Moveable(0, 20, 0, 180, shape, new MoveStrategy());
                        GameController.Player = new Player(opponent.Name, opponent.Name, opponent.Wallet, playerShip);
                        addOpponentToCanvas(shape);
                    } else {
                        GameController.Player.Wallet = opponent.Wallet;
                        GameController.Player.PlayerShip.Mover.Move(GameController.Player.PlayerShip, GameController.DeltaTime(GameController.tickTimer));
                    }
                }
            }
            return opponents;
        }

        internal static void moveOpponentOnCanvas(string username, PlayerDTO playerDTO) {
            if(GameController.Opponents != null && GameController.Opponents.ContainsKey(username)) {
                removeOpponentFromCanvas(playerDTO.Name);
                IRenderable renderable = new Renderable(Colors.Black, Colors.HotPink, 1, 25, 25, playerDTO.Vertices);
                Shape shape = new Shape(playerDTO.Name, playerDTO.Ray, renderable, new RenderWithHeaderStrategy());
                shape.Renderer.Render(shape.Renderable, shape.Ray);
                if(GameController.Opponents.TryAdd(username, shape)) {
                    addOpponentToCanvas(shape);
                }
            }
        }

        internal static void removeOpponentFromCanvas(string username) {
            ThreadController.MainThreadDispatcher.Invoke(() => {
                if(GameController.Opponents.TryRemove(username, out IShape r)) {
                    ArenaController.ArenaCanvas.Children.Remove(r.Polygon);
                }
            });
        }

        public static void addOpponentToCanvas(IShape shape) {
            ThreadController.MainThreadDispatcher.Invoke(() => {
                ArenaController.ArenaCanvas.Children.Add(shape.Polygon);
            });
        }
    }
}
