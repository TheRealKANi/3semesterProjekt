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
    }
}
