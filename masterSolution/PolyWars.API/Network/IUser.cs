namespace PolyWars.API.Network {
    public interface IUser {
        string ID { get; set; }
        string Name { get; set; }
        string HashedPassword { get; set; }
        bool IsLoggedIn { get; set; }
    }
}