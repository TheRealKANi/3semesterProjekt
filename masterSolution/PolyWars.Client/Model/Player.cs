using PolyWars.API.Model.Interfaces;

namespace PolyWars.Client.Model {
    
    ///
    /// Base class for the local clients Player
    /// </summary>
    public class Player : IPlayer {
        public Player(string name, string id, double currency, int health, IMoveable playerShip) {
            Name = name;
            ID = id;
            Wallet = currency;
            Health = health;
            PlayerShip = playerShip;
        }
        public string Name { get; set; }
        public string ID { get; private set; }
        public double Wallet { get; set; }
        public IMoveable PlayerShip { get; private set; }
        public int Health { get; set; }
    }
}
