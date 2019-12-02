using PolyWars.API;

namespace PolyWars.ServerClasses {

    class Resource : IResource {
        public IShape Shape { get; private set; }
        public double Value { get; private set; }

        public Resource(IShape shape, double value) {
            Shape = shape;
            Value = value;
        }
    }
}
