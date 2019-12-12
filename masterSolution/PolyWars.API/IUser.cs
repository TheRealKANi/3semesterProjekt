namespace PolyWars.API {
    public interface IUser {

        string HashedPassword { get; set; }
        string Name { get; set; }
        string ID { get; set; }
    }
}