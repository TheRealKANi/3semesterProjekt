using PolyWars.API.Strategies;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PolyWars.API.Model.Interfaces {

    /// <summary>
    /// Interface class for Shape that contains all the main methods 
    /// </summary>
    public interface IShape {
        IRay Ray { get; set; }
        IRenderable Renderable { get; set; }
        IRenderStrategy Renderer { get; }
        Polygon Polygon { get; }
    }
}
