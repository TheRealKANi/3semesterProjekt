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

        public static void move( IShape shape, Dispatcher dispatcher ) {
            RotateTransform rt = new RotateTransform {
                CenterX = shape.CenterPoint.X,
                CenterY = shape.CenterPoint.Y,
                Angle = shape.Angle
            };
            Polygon p = shape.getShapeAsPolygon( dispatcher );
            
            dispatcher.Invoke( () => {
                p.RenderTransform = rt;
                p.Stroke = Brushes.Blue;
                p.StrokeThickness = 10;
            } );
            
        }
    }
}
