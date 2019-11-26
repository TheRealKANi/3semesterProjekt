using PolyWars.API;
using PolyWars.Model;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PolyWars.Logic {

    class Resource : Model.Shape, IResource{

        public double ResourceValue { get; set; }

        public Resource( Point centerPoint, Int32 angle, ShapeSize size, Double velocity, Double maxVelocity, Double rps, Double maxRPS, double resourceValue ) :
                base( centerPoint, angle, Colors.Black, Colors.ForestGreen, size, velocity, maxVelocity, rps, maxRPS ) {
            Polygon.Points = generateResourcePoints();
            ResourceValue = resourceValue;
        }

        public PointCollection generateResourcePoints() {
            Double halfWidth = Size.Width / 2;
            Double halfHight = Size.Height / 2;
            Point topLeft = new Point( CenterPoint.X - halfWidth, CenterPoint.Y - halfHight );
            Point botLeft = new Point( CenterPoint.X - halfWidth, CenterPoint.Y + halfHight );
            Point botRight = new Point( CenterPoint.X + halfWidth, CenterPoint.Y + halfWidth );
            Point topRight = new Point( CenterPoint.X + halfWidth, CenterPoint.Y - halfHight );
            return new PointCollection() { topLeft, botLeft, botRight, topRight };
        }
    }
}
