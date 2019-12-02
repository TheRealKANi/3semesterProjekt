using PolyWars.API;
using System.Windows.Media;

namespace PolyWars.ServerClasses {
    class Renderable : IRenderable {
        public Color BorderColor { get; set; }
        public Color FillColor { get; set; }
        public double StrokeThickness { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Vertices { get; set; }

        public Renderable(Color borderColor, Color fillColor, double strokeThickness, int width, int height, int vertices) {
            BorderColor = borderColor;
            FillColor = fillColor;
            StrokeThickness = strokeThickness;
            Width = width;
            Height = height;
            Vertices = vertices;
        }
    }
}
