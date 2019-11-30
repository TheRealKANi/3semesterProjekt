using PolyWars.API;
using PolyWars.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PolyWars.Logic {
    class ArenaController {
        void fillArena(IPlayer player, IEnumerable<IResource> resources) {

        }
        /// <summary>
        /// This method prepares the canvas with a color and adds triangle to it with given values
        /// </summary>
        /// <returns>
        /// Returns canvas with background color and triangle
        /// </returns>
        public void prepareGame(Canvas canvas, IPlayer player, IEnumerable<IMoveable> moveables, IEnumerable<IResource> resources) {
            Ticker = new Ticker();
            tickTimer = new Stopwatch();

            Player = player;
            Moveables = moveables;
            Resources = resources;
            // TODO getOpponents();

            // TODO DEBUG - Init Frame Timer
            FrameDebugTimer.initTimers();

            canvas.Children.Add(player.PlayerShip.Shape.Polygon);

            foreach(IShape shape in moveables.Select(x => x.Shape).Concat(Resources.Select(x => x.Shape))) {
                canvas.Children.Add(shape.Polygon);
            }

            isLoaded = true;
        }
    }
}
