namespace PolyWars.API.Model.Interfaces {

    /// <summary>
    /// Creates a basic player with an Shape
    /// </summary>
    public interface IPlayer {
        string Name { get; set; }
        string ID { get; }
        double Wallet { get; set; }
        int Health { get; set; }
        IMoveable PlayerShip { get; }
    }
}
