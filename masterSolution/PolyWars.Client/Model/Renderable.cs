using PolyWars.API.Model.Interfaces;
using System.Windows.Media;

namespace PolyWars.Client.Model {
    /// <summary>
    /// Base class for a entity that can be shown on anrena
    /// </summary>
    class Renderable : IRenderable {
        public double StrokeThickness { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Vertices { get; set; }
        public Color BorderColor { get; set; }
        public Color FillColor { get; set; }

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
