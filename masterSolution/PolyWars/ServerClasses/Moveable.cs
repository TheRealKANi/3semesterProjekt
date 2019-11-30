using PolyWars.API;
using PolyWars.API.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyWars.ServerClasses {
    class Moveable : IMoveable {
        public double Velocity { get; set; }
        public double MaxVelocity { get; }
        public double RPM { get; set; }
        public double MaxRPM { get; set; }
        public IShape Shape { get; private set; }
        public IMoveStrategy Mover { get; set; } 

        public Moveable(double velocity, double maxVelocity, double rPM, double maxRPM, IShape shape, IMoveStrategy mover) {
            Velocity = velocity;
            MaxVelocity = maxVelocity;
            RPM = rPM;
            MaxRPM = maxRPM;
            Shape = shape;
            Mover = mover;

            Mover.Move(this, 0);
        }
        public void Move(decimal deltaTime) {
            Mover.Move(this, deltaTime);
        }
    }
}
