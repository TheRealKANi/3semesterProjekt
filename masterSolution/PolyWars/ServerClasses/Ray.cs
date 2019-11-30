using PolyWars.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PolyWars.ServerClasses{
    class Ray : IRay {
        public int ID { get; set ; }
        public Point CenterPoint { get; set; }
        public double Angle { get; set; }

        public Ray(int iD, Point centerPoint, double angle) {
            ID = iD;
            CenterPoint = centerPoint;
            Angle = angle;
        }
    }
}
