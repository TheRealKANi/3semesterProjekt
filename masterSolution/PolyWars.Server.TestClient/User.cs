namespace PolyWars.Server {
    public class User {
        public User() {

        }
        public User(string name, string id) {
            this.Name = name;
            this.ID = id;
        }
        public string Name { get; set; }
        public string ID { get; set; }
    }
}

