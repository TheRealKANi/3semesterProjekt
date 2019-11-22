using PolyWars.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PolyWars.Model {

    abstract class Shape : IShape {

        public Point CenterPoint { get; set; }
        public int Angle { get; set; }
        public Color BorderColor { get; set; }
        public Color FillColor { get; set; }
        public IShapeSize Size { get; set; }
        public PointCollection Points { get; set; }
        public Double Velocity { get; set; }
        public Double MaxVelocity { get; private set; }


        public Shape( Point centerPoint, int angle, Color borderColor, Color fillColor, ShapeSize size, PointCollection points, double velocity, double maxVelocity ) {
            CenterPoint = centerPoint;
            Angle = angle;
            BorderColor = borderColor;
            FillColor = fillColor;
            Size = size;
            Points = points;
            Velocity = velocity;
            MaxVelocity = maxVelocity;
        }


        public Shape( Point centerPoint, int angle, Color borderColor, Color fillColor, ShapeSize size, double maxVelocity = 0, double velocity = 0 )
             : this( centerPoint, angle, borderColor, fillColor, size, new PointCollection(), velocity, maxVelocity ) { }


        public Polygon getShapeAsPolygon(Dispatcher dispatcher) {
            Polygon polygon = dispatcher.Invoke(() => {
                Polygon poly = new Polygon {
                    Points = this.Points
                };

                poly.Stroke = new SolidColorBrush(this.BorderColor);
                poly.Fill = new SolidColorBrush(this.FillColor);
                poly.StrokeThickness = 2;
                return poly;
            } );
            return polygon;
        }
    }
}
