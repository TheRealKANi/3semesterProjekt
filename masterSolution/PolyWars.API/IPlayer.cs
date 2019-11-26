using PolyWars.API;

namespace PolyWars.API {

    /// <summary>
    /// Creates a basic player with an Shape
    /// </summary>
    public interface IPlayer : IShape {
        double CurrencyWallet { get; set; }
    }
}
