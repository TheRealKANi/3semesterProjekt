using System.Windows;

namespace PolyWars.API.Model.Interfaces {
    public interface IRay {
        string ID { get; set; }
        Point CenterPoint { get; set; }
        double Angle { get; set; }
    }
}
