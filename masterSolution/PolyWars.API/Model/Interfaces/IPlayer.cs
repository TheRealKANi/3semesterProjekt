namespace PolyWars.API.Model.Interfaces {
    public interface IPlayer {
        string Name { get; set; }
        string ID { get; }
        double Wallet { get; set; }
        int Health { get; set; }
        IMoveable PlayerShip { get; }
    }
}
