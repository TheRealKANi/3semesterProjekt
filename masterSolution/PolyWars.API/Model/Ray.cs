using PolyWars.API.Model.Interfaces;
using System.Windows;

namespace PolyWars.Api.Model {
    public class Ray : IRay {
        public string ID { get; set; }
        public Point CenterPoint { get; set; }
        public double Angle { get; set; }

        public Ray(string iD, Point centerPoint, double angle) {
            ID = iD;
            CenterPoint = centerPoint;
            Angle = angle;
        }

        public override string ToString() {
            return $"id:{ID}, CenterPointX:{CenterPoint.X}Y:{CenterPoint.Y}, Angle:{Angle}";
        }
    }
}
