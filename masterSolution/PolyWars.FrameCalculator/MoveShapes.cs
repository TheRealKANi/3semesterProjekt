using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using PolyWars.API;

namespace PolyWars.FrameCalculator {
    public static class MoveShapes {

        public static void move( IShape shape, double timeFactor) {
            double offsetX = shape.Velocity * Math.Cos(shape.Angle * Math.PI / 180) * timeFactor;
            double offsetY = shape.Velocity * Math.Sin(shape.Angle * Math.PI / 180) * timeFactor;

            shape.CenterPoint.Offset(shape.CenterPoint.X, shape.CenterPoint.Y);
            foreach(Point point in shape.Polygon.Points) {
                point.Offset(offsetX, offsetY) ;
            }

            double newAngle = shape.Angle + shape.RPS / 60 * timeFactor;

            var rt = new RotateTransform(newAngle, shape.CenterPoint.X, shape.CenterPoint.Y);
            shape.Polygon.RenderTransform = rt;
        }
    }
}
