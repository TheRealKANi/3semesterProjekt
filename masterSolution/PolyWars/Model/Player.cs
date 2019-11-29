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
    public class Player : IPlayer {
        public Player( Point centerPoint, int angle, Color borderColor, Color fillColor, ShapeSize size, double velocity, double maxVelocity, double rps, double maxRPS ) { 
            Shape = new Triangle( centerPoint, 0, borderColor, fillColor, size, velocity, maxVelocity, rps, maxRPS );
            Shape.Polygon.RenderTransform = new RotateTransform( angle, centerPoint.X, centerPoint.Y ); //TODO doesn't work
            InputController = InputController.Instance;
            InputController.Instance.initInput( this );
            PlayerShape = Shape.GetType().Name;
        }
        public string Name { get; set; }
        public string ID { get; set; }
        public IShape Shape { get; set; }
        public double CurrencyWallet { get; set; }
        public string PlayerShape { get; set; }

        public InputController InputController { get; private set; }

    }
}
