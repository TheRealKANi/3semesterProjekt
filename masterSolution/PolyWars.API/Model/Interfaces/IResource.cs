namespace PolyWars.API.Model.Interfaces {
    public interface IResource {
        string ID { get; }
        IShape Shape { get; }
        double Value { get; }
        bool RequestedPickup { get; set; }
    }
}
