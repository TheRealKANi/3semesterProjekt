namespace PolyWars.API.Network {
    /// <summary>
    /// Base for transfering a user thru realtime client protocol
    /// </summary>
    public interface IUser {
        string ID { get; set; }
        string Name { get; set; }
        string HashedPassword { get; set; }
        bool IsLoggedIn { get; set; }
    }
}