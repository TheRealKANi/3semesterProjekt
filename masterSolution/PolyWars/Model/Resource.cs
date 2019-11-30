using PolyWars.API;
using PolyWars.Model;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PolyWars.Logic {

    class Resource : IResource {
        public IShape Shape { get; private set; }
        public double Value { get; private set; }
        
        public Resource( IShape shape, double value) {
            Shape = shape;
            Value = value;
        }
    }
}
