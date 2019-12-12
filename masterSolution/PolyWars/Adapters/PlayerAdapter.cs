using PolyWars.Api.Model;
using PolyWars.API.Model.Interfaces;
using PolyWars.API.Network.DTO;
using PolyWars.API.Strategies;
using PolyWars.Logic;
using PolyWars.Network;
using PolyWars.ServerClasses;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace PolyWars.Adapters {
    class PlayerAdapter {
        public static IMoveable playerDTOToMoveable(PlayerDTO dto) {
            IRenderable renderable = new Renderable(Colors.Black, dto.FillColor, 1, dto.Width, dto.Height, dto.Vertices);
            return playerDTOToMoveable(dto, renderable);
        }
        public static IMoveable playerDTOToMoveable(PlayerDTO dto, IRenderable renderable) {
            IRay ray = new Ray(dto.ID, new Point(dto.centerX, dto.centerY), dto.Angle);
            Shape shape = new Shape(dto.ID, ray, renderable, new RenderWithHeaderStrategy());
            return new Moveable(dto.Velocity, dto.MaxVelocity, dto.RPM, dto.MaxRPM, shape, new MoveOpponentStrategy());
        }
        public static IMoveable playerDTOToMoveable(PlayerDTO dto, IMoveable player) {
            IRay ray = new Ray(dto.ID, new Point(dto.centerX, dto.centerY), dto.Angle);
            player.Shape.Ray = ray;
            IMoveable moveable = new Moveable(dto.Velocity, dto.MaxVelocity, dto.RPM, dto.MaxRPM, player.Shape, player.Mover);
            return moveable;
        }
        public static PlayerDTO MoveableToPlayerDTO(IMoveable player) {
            return new PlayerDTO() {
                Name = GameController.Username,
                ID = player.Shape.Ray.ID,
                Velocity = player.Velocity,
                MaxVelocity = player.MaxVelocity,
                RPM = player.RPM,
                MaxRPM = player.MaxRPM,
                Vertices = player.Shape.Renderable.Vertices,
                centerX = player.Shape.Ray.CenterPoint.X,
                centerY = player.Shape.Ray.CenterPoint.Y,
                Angle = player.Shape.Ray.Angle,
                Height = player.Shape.Renderable.Height,
                Width = player.Shape.Renderable.Width,
                FillColor = player.Shape.Renderable.FillColor,
                Health = GameController.Player.Health
            };
        }
    }
}
