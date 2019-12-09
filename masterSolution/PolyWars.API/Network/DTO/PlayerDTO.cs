using PolyWars.Api.Model;
using System.Windows;

namespace PolyWars.API.Network.DTO {
    public class PlayerDTO {
        public string Name { get; set; }
        public string ID { get; set; }
        public double Wallet { get; set; }
        public double Velocity { get; set; }
        public double MaxVelocity { get; set; }
        public double RPM { get; set; }
        public double MaxRPM { get; set; }
        public int Vertices { get; set; }
        public double centerX { get; set; }
        public double centerY { get; set; }
        public double Angle { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Health { get; set; }
    }
}
