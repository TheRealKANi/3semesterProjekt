using PolyWars.Api.Model;
using PolyWars.API.Model.Interfaces;
using PolyWars.API.Network.DTO;
using PolyWars.API.Strategies;
using PolyWars.Logic;
using PolyWars.Network;
using PolyWars.Server.Model;
using PolyWars.ServerClasses;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace PolyWars.Adapters {
    class ResourceAdapter {

        public static IResource DTOToResource(ResourceDTO dto) {
            IRay ray = new Ray(dto.ID, new Point(dto.CenterX, dto.CenterY), dto.Angle);
            IRenderable renderable = new Renderable(Colors.Black, Colors.ForestGreen, 1, 15, 15, 4);
            IShape shape = new Shape(dto.ID, ray, renderable, new RenderStrategy());
            
            return new Resource(dto.ID, shape, dto.Value);
        }
        public static ResourceDTO IResourceToDTO(IResource resource) {
            return new ResourceDTO {
                ID = resource.ID,
                CenterX = resource.Shape.Ray.CenterPoint.X,
                CenterY = resource.Shape.Ray.CenterPoint.Y,
                Angle = resource.Shape.Ray.Angle,
                Value = resource.Value
            };
        }
    }
}
