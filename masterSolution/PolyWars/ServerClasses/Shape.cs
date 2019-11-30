using PolyWars.API;
using PolyWars.API.Strategies;
using System.Windows.Shapes;

namespace PolyWars.ServerClasses {

    class Shape : IShape  {
        public int ID { get; private set; }
        public IRay Ray { get; set; }
        public IRenderable Renderable { get; set; }
        public IRenderStrategy Renderer { get; private set; }
        public Polygon Polygon { get; private set; }


        public Shape(int id, IRay ray, IRenderable renderable, IRenderStrategy renderer) {
            ID = id;
            Ray = ray;
            Renderable = renderable;
            Renderer = renderer;

            Polygon = Renderer.Render(Renderable, Ray);
        }
    }
}
