using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using PolyWars.API;

namespace PolyWars.FrameCalculator {
    public static class MoveShapes {

        public static void move( IShape shape ) {
            RotateTransform rt = new RotateTransform {
                CenterX = shape.CenterPoint.X,
                CenterY = shape.CenterPoint.Y,
                Angle = shape.Angle
            };
            Polygon p = shape.getShapeAsPolygon();
            p.RenderTransform = rt;
        }
    }
}
