using PolyWars.API.Model.Interfaces;
using PolyWars.API.Strategies;
using PolyWars.Client.Logic;
using System.Windows.Shapes;

namespace PolyWars.ServerClasses {

    /// <summary>
    /// Base class for a Shape
    /// </summary>
    class Shape : IShape {
        public string ID { get; private set; }
        public IRay Ray { get; set; }
        public IRenderable Renderable { get; set; }
        public IRenderStrategy Renderer { get; private set; }
        public Polygon Polygon { get; private set; }

        public Shape(string id, IRay ray, IRenderable renderable, IRenderStrategy renderer) {
            ID = id;
            Ray = ray;
            Renderable = renderable;
            Renderer = renderer;
            UIDispatcher.Invoke(() => {
                Polygon = Renderer.Render(Renderable, Ray);
            });
        }
    }
}
