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

        public static Polygon move( IShape shape, Dispatcher dispatcher ) {
            RotateTransform rt = new RotateTransform( shape.Angle );
            rt.CenterX = shape.CenterPoint.X;
            rt.CenterY = shape.CenterPoint.Y;
            Polygon p = shape.getShapeAsPolygon( dispatcher );
            return p;
        }
    }
}
