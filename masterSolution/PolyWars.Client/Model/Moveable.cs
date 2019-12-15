using PolyWars.API.Model.Interfaces;
using PolyWars.API.Strategies;

namespace PolyWars.Client.Model {
    /// <summary>
    /// Base class for a Moveable entity
    /// </summary>
    class Moveable : IMoveable {
        public double Velocity { get; set; }
        public double MaxVelocity { get; set; }
        public double RPM { get; set; }
        public double MaxRPM { get; set; }
        public IShape Shape { get; set; }
        public IMoveStrategy Mover { get; set; }

        public Moveable(double velocity, double maxVelocity, double rPM, double maxRPM, IShape shape, IMoveStrategy mover) {
            Velocity = velocity;
            MaxVelocity = maxVelocity;
            RPM = rPM;
            MaxRPM = maxRPM;
            Shape = shape;
            Mover = mover;
        }

        public void Move(double deltaTime) {
            Mover.Move(this, deltaTime);
        }
    }
}
