using PolyWars.API;
using PolyWars.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PolyWars.Model {
    class Player : IPlayer {
        public Player( Point centerPoint, int angle, Color borderColor, Color fillColor, ShapeSize size, double velocity, double maxVelocity, double rps, double maxRPS ) { 
            Shape = new Triangle( centerPoint, angle, borderColor, fillColor, size, velocity, maxVelocity, rps, maxRPS );
        }
        public IShape Shape { get; set; }
        public double CurrencyWallet { get; set; }
    }
}
