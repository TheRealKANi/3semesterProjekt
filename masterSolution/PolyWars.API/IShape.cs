﻿using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PolyWars.API {

    /// <summary>
    /// Interface class for Shape that contains all the main methods 
    /// </summary>
    public interface IShape {

        Point CenterPoint { get; set; }
        double Angle { get; set; }
        Color BorderColor { get; set; }
        Color FillColor { get; set; }
        IShapeSize Size { get; set; }
        Polygon Polygon { get; set; }
        double Velocity { get; set; }
        double MaxVelocity { get; }
        double RPS { get; set; }
        double MaxRPS { get; set; }
    }
}
