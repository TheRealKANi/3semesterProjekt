using PolyWars.API.Model.Interfaces;
using System.Windows.Shapes;

namespace PolyWars.API.Strategies {
    public interface IRenderStrategy {
        Polygon Render(IRenderable r, IRay ray);
    }
}
