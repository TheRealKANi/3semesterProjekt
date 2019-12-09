using PolyWars.Api.Model;

namespace PolyWars.API.Network.DTO {
    public class PlayerDTO {
        public string Name { get; set; }
        public string ID { get; set; }
        public double Wallet { get; set; }
        public int Health { get; set; }
        public int Vertices { get; set; }
        public Ray Ray { get; set; }
    }
}
