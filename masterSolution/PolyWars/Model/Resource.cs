using PolyWars.API;
using PolyWars.Model;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PolyWars.Logic {

    class Resource : Model.Shape, IResource{
        private static int idCounter;
        //public int ID { get; private set; }
        public double ResourceValue { get; set; }
        
        public Resource( Point centerPoint, int angle, ShapeSize size, double resourceValue ) : 
            this(centerPoint, angle, size, 0, 0, 0, 0, resourceValue ) { }

        public Resource( Point centerPoint, int angle, ShapeSize size, double velocity, double maxVelocity, double rpm, double maxRPM, double resourceValue ) :
                base( centerPoint, angle, Colors.Black, Colors.ForestGreen, size, velocity, maxVelocity, rpm, maxRPM ) {
            Polygon.Points = generateResourcePoints();
            ResourceValue = resourceValue;
            ID = idCounter++;
        }

        public PointCollection generateResourcePoints() {
            double halfWidth = Size.Width / 2;
            double halfHight = Size.Height / 2;
            Point topLeft = new Point( CenterPoint.X - halfWidth, CenterPoint.Y - halfHight );
            Point botLeft = new Point( CenterPoint.X - halfWidth, CenterPoint.Y + halfHight );
            Point botRight = new Point( CenterPoint.X + halfWidth, CenterPoint.Y + halfWidth );
            Point topRight = new Point( CenterPoint.X + halfWidth, CenterPoint.Y - halfHight );
            return new PointCollection() { topLeft, botLeft, botRight, topRight };
        }

        public override bool Equals( object obj ) {
            if( obj is Resource resource ) {
                return ID == resource.ID;
            } else {
                return false;
            }
        }
        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
}
