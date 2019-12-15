using PolyWars.API.Model;

namespace PolyWars.API.Network.DTO {
    /// <summary>
    /// Base class for transport of a bullet thru network to the server
    /// </summary>
    public class BulletDTO {
        public string ID { get; set; }
        public string PlayerID { get; set; }
        public Ray Ray { get; set; }
        public int Damage { get; set; }
        public override string ToString() {
            return "PlayerID: " + PlayerID.ToString() + ", ID: " + ID.ToString() + ", Damage: " + Damage.ToString();
        }
    }
}

