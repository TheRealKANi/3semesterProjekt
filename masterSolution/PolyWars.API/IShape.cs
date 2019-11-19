using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PolyWars.API {
    public interface IShape {
        Polygon getShapeAsPolygon();
        Point CenterPoint { get; set; }
        int Angle { get; set; }
        Color BorderColor { get; set; }
        Color FillColor { get; set; }
        IShapeSize Size { get; set; }
        PointCollection Points { get; set; }
    }
}
