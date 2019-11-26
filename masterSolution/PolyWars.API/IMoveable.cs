namespace PolyWars.API {
    public interface IMoveable {
        double Velocity { get; set; }
        double MaxVelocity { get; }
        double RPM { get; set; }
        double MaxRPM { get; set; }
    }
}