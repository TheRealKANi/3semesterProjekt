using PolyWars.API.Model.Interfaces;
using System;
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

        public Ray Clone() {
            return new Ray(ID + "", new Point(CenterPoint.X, CenterPoint.Y), Double.Parse(Angle.ToString()));
        }

        public bool IsEqual(IRay other) {
            bool result = false;
            Point thisPoint = new Point((int) CenterPoint.X, (int) CenterPoint.Y);
            Point otherPoint = new Point((int) other.CenterPoint.X, (int) other.CenterPoint.Y);
            int thisAngle = (int) Angle;
            int otherAngle = (int) other.Angle;
            if(other.ID.Equals(ID) &&
                otherPoint.Equals(thisPoint)
                && otherAngle == thisAngle) {
                result = true;
            }
            return result;
        }
    }
}
