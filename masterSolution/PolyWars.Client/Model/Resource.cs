using PolyWars.API.Model.Interfaces;

namespace PolyWars.Client.Model {
    /// <summary>
    /// Base class for a Resource
    /// </summary>
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
