using PolyWars.API;
using PolyWars.API.Model.Interfaces;
using PolyWars.Logic;

namespace PolyWars.ServerClasses {
    public class Player : IPlayer {
        public Player(string name, string id, double currency, IMoveable playerShip) {
            Name = name;
            ID = id;
            Wallet = currency;
            PlayerShip = playerShip;
            InputController = InputController.Instance;
            InputController.Instance.initInput(this);
        }
        public string Name { get; set; }
        public string ID { get; private set; }
        public double Wallet { get; set; }
        public IMoveable PlayerShip { get; private set; }

        public InputController InputController { get; private set; }

    }
}
