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

namespace PolyWars.ServerClasses {
    public class Player : IPlayer {
        public Player(string name, string id, double currency, IMoveable playerShip) {
            Name = name;
            ID = id;
            Wallet = currency;
            PlayerShip = playerShip;
            InputController = InputController.Instance;
            InputController.Instance.initInput( this );
        }
        public string Name { get; set; }
        public string ID { get; private set; }
        public double Wallet { get; set; }
        public IMoveable PlayerShip { get; private set; }

        public InputController InputController { get; private set; }

    }
}
