using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PolyWars.Logic {
    static class ArenaController {

        public static Canvas ArenaCanvas { get; private set; }
        public static int ArenaBoundWidth { get; set; }
        public static int ArenaBoundHeight { get; set; }

        public static void generateCanvas() {
            ArenaCanvas = new Canvas {
                Background = new SolidColorBrush(Colors.Aquamarine)
            };
            System.Windows.Shapes.Rectangle rect;
            rect = new System.Windows.Shapes.Rectangle {
                Stroke = new SolidColorBrush(Colors.Black),
                Width = ArenaBoundWidth,
                StrokeThickness = 2,
                Height = ArenaBoundHeight
            };
            Canvas.SetLeft(rect, 0);
            Canvas.SetTop(rect, 0);
            ArenaCanvas.Children.Add(rect);
        }
    }
}
