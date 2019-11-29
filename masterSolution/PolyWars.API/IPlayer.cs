using PolyWars.API;

namespace PolyWars.API {

    /// <summary>
    /// Creates a basic player with an Shape
    /// </summary>
    public interface IPlayer {
        string Name { get; set; }
        string ID { get; set; }
        double CurrencyWallet { get; set; }
        IShape Shape { get; set; }
        string PlayerShape { get; set; }
    }
}
