using System.Windows.Media;

namespace PolyWars.API {
    public interface IRenderable {
        Color BorderColor { get; set; }
        Color FillColor { get; set; }
        double StrokeThickness { get; set; }
        int Width { get; set; }
        int Height { get; set; }
        int Vertices { get; set; }
    }
}
