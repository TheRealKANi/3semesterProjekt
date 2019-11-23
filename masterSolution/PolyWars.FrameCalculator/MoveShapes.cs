using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            shape.Angle += shape.RPS * 6 * timeFactor;
            shape.Polygon.RenderTransform = new RotateTransform(-1 * shape.Angle, shape.CenterPoint.X, shape.CenterPoint.Y);

            double offsetX = shape.Velocity * Math.Sin(shape.Angle * Math.PI / 180) * timeFactor;
            double offsetY = shape.Velocity * Math.Cos(shape.Angle * Math.PI / 180) * timeFactor;

            Point cp = shape.CenterPoint;
            cp.Offset(offsetX, offsetY);
            shape.CenterPoint = cp;

            PointCollection pc = shape.Polygon.Points;
            for (int i = 0; i < pc.Count; i++){
                Point p = pc[i];
                p.Offset(offsetX, offsetY);
                pc[i] = p;
            }

            
        }
    }
}
