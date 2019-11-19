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
        public Shape(Point centerPoint, int angle, Color borderColor, Color fillColor, ShapeSize size, PointCollection points) {
            CenterPoint = centerPoint;
            Angle = angle;
            BorderColor = borderColor;
            FillColor = fillColor;
            Size = size;
            Points = points;
        }

        public Shape(Point centerPoint, int angle, Color borderColor, Color fillColor, ShapeSize size){
            this.CenterPoint = centerPoint;
            this.Angle = angle;
            this.BorderColor = borderColor;
            this.FillColor = fillColor;
            this.Size = size;
            this.Points = new PointCollection();

        }

        public Point CenterPoint { get; set; }
        public int Angle { get; set; }
        public Color BorderColor { get; set; }
        public Color FillColor { get; set; }
        public IShapeSize Size { get; set; }
        public PointCollection Points { get; set; }

        public Polygon getShapeAsPolygon(Dispatcher dispatcher) {
            return dispatcher.Invoke( () => {
                Polygon poly = new Polygon {
                    Points = this.Points
                };
                return poly;
            } );
        }

    }
}
