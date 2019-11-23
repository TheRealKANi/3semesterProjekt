using PolyWars.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace PolyWars.Model {

    class Triangle : Shape {
        /// <summary>
        /// Contains the collection of instructions that are executed at the time of Object creation.
        /// </summary>
        public Triangle(Point centerPoint, int angle, Color borderColor, Color fillColor, ShapeSize size, double velocity, double maxVelocity, double rps, double maxRPS) : 
            base(centerPoint, angle, borderColor, fillColor, size, velocity, maxVelocity, rps, maxRPS) {
            Polygon.Points = generateTrianglePoints();
            Angle += 180;
        }

        // TODO Refactor Code to external class
        /// <summary>
        /// This is the position for the triangles attachment points.
        /// </summary>
        private PointCollection generateTrianglePoints() {
            double x1 = this.CenterPoint.X + Math.Cos(3d / 2 * Math.PI + Angle / 180 * Math.PI) * this.Size.Width / 2;
            double y1 = this.CenterPoint.Y - Math.Sin(3d / 2 * Math.PI + Angle / 180 * Math.PI) * this.Size.Height / 2;
            Point point1 = new Point(x1, y1);

            double x2 = this.CenterPoint.X + Math.Cos((Angle + 30) * Math.PI / 180) * this.Size.Width / 2;
            double y2 = this.CenterPoint.Y - Math.Sin((Angle + 30) * Math.PI / 180) * this.Size.Height / 2;
            Point point2 = new Point(x2, y2);

            double x3 = this.CenterPoint.X + Math.Cos(Math.PI / 2 + (Angle + 60) * Math.PI / 180) * this.Size.Width / 2;
            double y3 = this.CenterPoint.Y - Math.Sin(Math.PI / 2 + (Angle + 60) * Math.PI / 180) * this.Size.Height / 2;
            Point point3 = new Point(x3, y3);

            return new PointCollection() { point1, point2, point3 };
        }
    }
}
