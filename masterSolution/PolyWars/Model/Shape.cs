using PolyWars.API;
using PolyWars.Logic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PolyWars.Model {

    abstract class Shape : IShape {

        public Point CenterPoint { get; set; }
        public double Angle { get; set; }
        public Color BorderColor { get; set; }
        public Color FillColor { get; set; }
        public IShapeSize Size { get; set; }
        public Polygon Polygon { get; set; }
        public double Velocity { get; set; }
        public double MaxVelocity { get; private set; }
        public double RPM { get; set; }
        public double MaxRPM { get; set; }
        public int deltaTime;

        public Shape( Point centerPoint, int angle, Color borderColor, Color fillColor, ShapeSize size, double velocity, double maxVelocity, double rpm, double maxRPM ) {
            CenterPoint = centerPoint;
            Angle = angle;
            BorderColor = borderColor;
            FillColor = fillColor;
            Size = size;
            Velocity = velocity;
            MaxVelocity = maxVelocity;
            RPM = rpm;
            MaxRPM = maxRPM;

            generatePolygon();
            Polygon.RenderTransform = new RotateTransform( -1 * angle, centerPoint.X, centerPoint.Y );
        }
        private void generatePolygon() {
            ThreadController.MainThreadDispatcher.Invoke( () => {
                Polygon = new Polygon() {
                    Stroke = new SolidColorBrush( this.BorderColor ),
                    Fill = new SolidColorBrush( this.FillColor ),
                    StrokeThickness = 1,
                };
            } );
        }
    }
}
