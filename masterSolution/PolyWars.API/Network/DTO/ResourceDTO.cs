namespace PolyWars.API.Network.DTO {
    /// <summary>
    /// Base class for transport of a Resource thru network to the server
    /// </summary>
    public class ResourceDTO {
        public string ID { get; set; }
        public double Value { get; set; }
        public double CenterX { get; set; }
        public double CenterY { get; set; }
        public double Angle { get; set; }
        public override string ToString() {
            return ID.ToString() + Value.ToString();
        }
    }
}
