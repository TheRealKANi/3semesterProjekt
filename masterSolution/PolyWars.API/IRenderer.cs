using PolyWars.API.Strategies;

namespace PolyWars.API {
    public interface IRenderer {
        IRenderStrategy Renderer { get; }
    }
}
