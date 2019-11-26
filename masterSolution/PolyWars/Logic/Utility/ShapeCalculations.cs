using PolyWars.API;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace PolyWars.Logic.Utility {
    static class ShapeCalculations {
        public static void moveShape( IShape shape, double timeFactor ) {
            shape.Angle += shape.RPM * 360 / ( 60 * 60 ) * timeFactor;
            double offsetX = shape.Velocity * Math.Sin( shape.Angle * Math.PI / 180 ) * timeFactor;
            double offsetY = shape.Velocity * Math.Cos( shape.Angle * Math.PI / 180 ) * timeFactor;

            Point cp = shape.CenterPoint;
            cp.Offset( offsetX, offsetY );
            shape.CenterPoint = cp;

            shape.Polygon.RenderTransform = new RotateTransform( -1 * shape.Angle, shape.CenterPoint.X, shape.CenterPoint.Y ); //TODO only if it actually rotates?

            PointCollection pc = shape.Polygon.Points;
            for( int i = 0; i < pc.Count; i++ ) {
                Point p = pc[i];
                p.Offset( offsetX, offsetY );
                pc[i] = p;
            }
        }
    }
}
