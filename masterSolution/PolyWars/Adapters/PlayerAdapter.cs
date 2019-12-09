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
            foreach(PlayerDTO playerDTO in opponentDTOs) {
                if(!playerDTO.Name.Equals(GameController.Username)) {
                    // Initial render of an opponent when game is launched
                    Shape shape = renderOpponent(playerDTO);
                    if(opponents.TryAdd(playerDTO.Name, shape)) {
                        addOpponentToCanvas(shape);
                    }
                } else {
                    // Initial render of clients Player when game is launched
                    if(GameController.Player == null) {
                        IShape shape = renderPlayer(playerDTO);
                        addOpponentToCanvas(shape);
                    }
                }
            }
            return opponents;
        }

        private static IShape renderPlayer(PlayerDTO opponent) {
            IRenderable renderable = new Renderable(Colors.Black, Colors.Gray, 1, 25, 25, opponent.Vertices);
            IShape shape = new Shape(opponent.Name, opponent.Ray, renderable, new RenderWithHeaderStrategy());
            IMoveable playerShip = new Moveable(0, 180, 0, 360, shape, new MoveStrategy());
            GameController.Player = new Player(opponent.Name, opponent.Name, opponent.Wallet, opponent.Health, playerShip);
            return shape;
        }

        private static Shape renderOpponent(PlayerDTO opponent) {
            IRenderable renderable = new Renderable(Colors.Black, Colors.DarkSlateGray, 1, 25, 25, opponent.Vertices);
            Shape shape = new Shape(opponent.Name, opponent.Ray, renderable, new RenderWithHeaderStrategy());
            shape.Renderer.Render(shape.Renderable, shape.Ray);
            return shape;
        }

        /// <summary>
        /// 'Moves' a opponent around on the arena based on the input DTO
        /// </summary>
        /// <param name="opponentDTO">The DTO of the user from server</param>
        internal static void moveOpponentOnCanvas(PlayerDTO opponentDTO) {
            if(GameController.Opponents != null && GameController.Opponents.ContainsKey(opponentDTO.Name)) {
                if(GameController.Opponents.TryRemove(opponentDTO.Name, out IShape opponentShape)) {
                    removeOpponentFromCanvas(opponentShape);
                    IRenderable renderable = new Renderable(Colors.Black, Colors.HotPink, 1, 25, 25, opponentDTO.Vertices);
                    Shape shape = new Shape(opponentDTO.Name, opponentDTO.Ray, renderable, new RenderWithHeaderStrategy());
                    if(GameController.Opponents.TryAdd(opponentDTO.Name, shape)) {
                        addOpponentToCanvas(shape);
                    }
                    //IMoveable opponentShip = new Moveable(0, 80, 0, 360, shape, new MoveStrategy());
                    //opponentShip.Mover.Move(opponentShip, GameController.DeltaTime(GameController.tickTimer));
                }
            }
        }

        internal static void removeOpponentFromCanvas(IShape shape) {
            ThreadController.MainThreadDispatcher.Invoke(() => {
                ArenaController.ArenaCanvas.Children.Remove(shape.Polygon);
            });
        }

        public static void addOpponentToCanvas(IShape shape) {
            ThreadController.MainThreadDispatcher.Invoke(() => {
                ArenaController.ArenaCanvas.Children.Add(shape.Polygon);
            });
        }
    }
}
