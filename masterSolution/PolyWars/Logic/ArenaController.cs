using PolyWars.API;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace PolyWars.Logic {
    static class ArenaController {
        public static void fillArena(Canvas canvas, IPlayer player, IEnumerable<IShape> immovables, IEnumerable<IResource> resources) {
            canvas.Children.Add(player.PlayerShip.Shape.Polygon);

            foreach(IShape shape in immovables.Concat(resources.Select(x => x.Shape))) {
                canvas.Children.Add(shape.Polygon);
            }
        }
    }
}
