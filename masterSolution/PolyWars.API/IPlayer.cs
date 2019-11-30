using PolyWars.API;

namespace PolyWars.API {

    /// <summary>
    /// Creates a basic player with an Shape
    /// </summary>
    public interface IPlayer {
        string Name { get; set; }
        string ID { get; }
        double Wallet { get; set; }
        IMoveable PlayerShip { get; }
    }
}
