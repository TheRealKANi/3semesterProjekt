using PolyWars.API.Strategies;
using System.Windows.Shapes;

namespace PolyWars.API.Model.Interfaces {
    public interface IShape {
        IRay Ray { get; set; }
        IRenderable Renderable { get; set; }
        IRenderStrategy Renderer { get; }
        Polygon Polygon { get; }
    }
}
