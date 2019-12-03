using PolyWars.API;
using PolyWars.API.Model.Interfaces;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;

namespace PolyWars.Logic {
    static class ArenaController {

        public static Canvas ArenaCanvas { get; private set; }

        public static void generateCanvas() {
            ArenaCanvas = new Canvas {
                Background = new SolidColorBrush(Colors.Aquamarine)
            };
        }

        public static void fillArena() {
            foreach(IShape shape in GameController.Immovables.Concat(GameController.Resources.Values.Select(x => x.Shape))) {
                ArenaCanvas.Children.Add(shape.Polygon);
            }
            // Player goes on top of all other IShapes
            ArenaCanvas.Children.Add(GameController.Player.PlayerShip.Shape.Polygon);
        }

    }
}
