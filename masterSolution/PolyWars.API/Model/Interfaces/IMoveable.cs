using PolyWars.API.Strategies;

namespace PolyWars.API.Model.Interfaces {
    public interface IMoveable {
        double Velocity { get; set; }
        double MaxVelocity { get; }
        double RPM { get; set; }
        double MaxRPM { get; set; }

        IShape Shape { get; set; }
        IMoveStrategy Mover { get; }

        void Move(decimal deltaTime);
    }
}