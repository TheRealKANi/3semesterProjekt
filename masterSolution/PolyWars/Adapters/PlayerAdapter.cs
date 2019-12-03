using PolyWars.API.Model.Interfaces;
using PolyWars.API.Network.DTO;
using PolyWars.Logic;
using PolyWars.Network;
using PolyWars.ServerClasses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PolyWars.Adapters {
    class PlayerAdapter {
        public static InputController InputController { get; private set; }

        public static async Task<List<IShape>> OpponentsDTOAdapter() {
            List<PlayerDTO> opponentsDTOs = await NetworkController.GameService.getOpponentsAsync();
            Console.WriteLine("Got Opponents from server");

            return PlayerDTOtoIShape(opponentsDTOs);
        }

        public static List<IShape> PlayerDTOtoIShape(List<PlayerDTO> opponentDTOs) {
            List<IShape> opponents = new List<IShape>();
            foreach(PlayerDTO opponent in opponentDTOs) {
                if(!opponent.Name.Equals(GameController.Player.Name)) {
                    IRenderable renderable = new Renderable(Colors.Black, Colors.OrangeRed, 1, 25, 25, opponent.Vertices);
                    opponents.Add(new Shape(opponent.ID, opponent.Ray, renderable, new RenderWithHeaderStrategy()));
                } else {
                    IRenderable renderable = new Renderable(Colors.Black, Colors.Gray, 1, 25, 25, opponent.Vertices);
                    //IShape shape = new Shape("0", ray, renderable, new RenderWithHeaderStrategy());
                    //IMoveable playerShip = new Moveable(0, 20, 0, 180, shape, new MoveStrategy());
                    //GameController.Player = new Player(opponent.Name, opponent.ID, opponent.Wallet, playerShip);
                    GameController.Player.PlayerShip.Shape.Ray = opponent.Ray;
                    GameController.Player.PlayerShip.Shape.Renderable = renderable;
                }
            }
            return opponents;
        }
    }
}
