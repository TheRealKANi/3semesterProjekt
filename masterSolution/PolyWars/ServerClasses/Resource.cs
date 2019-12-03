using PolyWars.API;
using PolyWars.API.Model.Interfaces;

namespace PolyWars.Server.Model {

    class Resource : IResource {
        public string ID { get; private set; }
        public IShape Shape { get; private set; }
        public double Value { get; private set; }

        public Resource(string id, IShape shape, double value) {
            ID = id;
            Shape = shape;
            Value = value;
        }
    }
}
