using PolyWars.API.Model.Interfaces;
using System.Windows;

namespace PolyWars.API.Model {
    /// <summary>
    /// Main Ray class that contains two most
    /// importent data for client server exchange.
    /// Angle and current location
    /// </summary>
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
            return $"id:{ID}, CenterPoint({CenterPoint.X},{CenterPoint.Y}), Angle:{Angle}";
        }

        public Ray Clone() {
            return new Ray(ID + "", new Point(CenterPoint.X, CenterPoint.Y), Angle);
        }
        /// <summary>
        /// Compaires Rays, with ID and nearest whole int rounding,
        /// to decrese movement frequency sent to the server.
        /// </summary>
        /// <param name="obj">The other ray to compaire to</param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            if(obj is IRay ray) {
                return (int) CenterPoint.X == (int) ray.CenterPoint.X && (int) CenterPoint.Y == (int) ray.CenterPoint.Y && (int) Angle == (int) ray.Angle && ID.Equals(ray.ID);
            }
            return false;
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
}
