using PolyWars.API.Strategies;

namespace PolyWars.API.Model.Interfaces {
    public interface IRenderer {
        IRenderStrategy Renderer { get; }
    }
}
