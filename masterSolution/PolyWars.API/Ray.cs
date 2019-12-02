using PolyWars.API;
using System.Windows;

namespace PolyWars.Api {
    public class Ray : IRay {
        public int ID { get; set; }
        public Point CenterPoint { get; set; }
        public double Angle { get; set; }

        public Ray(int iD, Point centerPoint, double angle) {
            ID = iD;
            CenterPoint = centerPoint;
            Angle = angle;
        }

        public override string ToString() {
            return $"id:{ID}, CenterPointX:{CenterPoint.X}Y:{CenterPoint.Y}, Angle:{Angle}";
        }
    }
}
