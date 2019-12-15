using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PolyWars.Client.Logic {
    static class ArenaController {
        public static Canvas ArenaCanvas { get; private set; }
        public static int ArenaBoundWidth { get; set; }
        public static int ArenaBoundHeight { get; set; }

        public static void generateCanvas() {
            ArenaCanvas = new Canvas {
                Background = new SolidColorBrush(Colors.Aquamarine)
            };
            Rectangle rect = generateVisualArenaBound(Colors.Black);
            ArenaCanvas.Children.Add(rect);
        }

        private static Rectangle generateVisualArenaBound(Color lineColor) {
            Rectangle rect = new Rectangle {
                Stroke = new SolidColorBrush(lineColor),
                Width = ArenaBoundWidth,
                StrokeThickness = 2,
                Height = ArenaBoundHeight
            };
            Canvas.SetLeft(rect, 0);
            Canvas.SetTop(rect, 0);
            return rect;
        }
    }
}
