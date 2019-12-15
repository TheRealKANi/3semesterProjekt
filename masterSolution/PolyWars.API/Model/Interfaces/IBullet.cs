namespace PolyWars.API.Model.Interfaces {
    public interface IBullet {
        string ID { get; }
        string PlayerID { get; }
        IMoveable BulletShip { get; }
        int Damage { get; }

    }
}
