using PolyWars.API.Network;

namespace PolyWars.API.Model {
    public class User : IUser {
        public string HashedPassword { get; set; }
        public string Name { get; set; }
        public string ID { get; set; }
        public bool IsLoggedIn { get; set; }

        public User(string name, string id, string hashedPassword) {
            Name = name;
            ID = id;
            HashedPassword = hashedPassword;
            IsLoggedIn = false;
        }
    }
}
