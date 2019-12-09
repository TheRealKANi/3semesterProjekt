using PolyWars.API.Strategies;

namespace PolyWars.API.Model.Interfaces {
    public interface IMoveable {
        double Velocity { get; set; }
        double MaxVelocity { get; set; }
        double RPM { get; set; }
        double MaxRPM { get; set; }

        IShape Shape { get; set; }
        IMoveStrategy Mover { get; set; }

        void Move(decimal deltaTime);
        //void MoveToNewRay();
    }
}