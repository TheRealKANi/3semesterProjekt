using PolyWars.API;

namespace PolyWars.Server {
    public class User : IUser {

        public string HashedPassword { get; set; }
        public string Name { get; set; }
        public string ID { get; set; }

        public User() {
        }

        public User(string name, string id) : this(name, id, "") {
        }

        public User(string name, string id, string hashedPassword ) {
            this.Name = name;
            this.ID = id;
            this.HashedPassword = hashedPassword;
        }
    }
}
