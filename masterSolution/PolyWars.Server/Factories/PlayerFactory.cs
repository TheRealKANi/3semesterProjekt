using PolyWars.API.Network;
using PolyWars.API.Network.DTO;
using System;
using System.Windows.Media;

namespace PolyWars.Server.Factories {
    class PlayerDTOFactory {
        private static Random r;

        static PlayerDTOFactory() {
            r = new Random();
        }

        public static PlayerDTO GetPlayerDTO(IUser newUser) {
            return new PlayerDTO() {
                ID = newUser.ID,
                Name = newUser.Name,
                centerX = r.Next(50, 500),
                centerY = r.Next(50, 500),
                Angle = 0,
                Velocity = 0,
                MaxVelocity = 15,
                RPM = 0,
                MaxRPM = 60,
                Vertices = 3,
                Wallet = 0,
                Width = 50,
                Height = 50,
                Health = 100,
                FillColor = GetColor()
            };
        }
        private static Color GetColor() {
            Color c = Color.FromArgb(255, (byte) r.Next(64, 256), (byte) r.Next(64, 256), (byte) r.Next(64, 256));
            return c;
        }
    }
}
