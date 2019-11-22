using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace PolyWars.API {
    /// <summary>
    /// Interface class for Trianlge that contains all the main methods and properties
    /// </summary>
    public interface ITriangle : IShape {
        PointCollection getTrianglePoints();
        double RPM { get; set; }
        int VerticalSpeed { get; set; }
        int HorizontialSpeed { get; set; }
    }
}
