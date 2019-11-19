using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace PolyWars.API {
    public interface ITriangle : IShape {
        PointCollection getTrianglePoints();
        int RPM { get; set; }
        int VerticalSpeed { get; set; }
        int HorizontialSpeed { get; set; }
    }
}
