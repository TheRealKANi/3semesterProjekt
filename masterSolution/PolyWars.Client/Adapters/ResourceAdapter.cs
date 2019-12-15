using PolyWars.API.Model;
using PolyWars.API.Model.Interfaces;
using PolyWars.API.Network.DTO;
using PolyWars.Client.Model;
using PolyWars.ServerClasses;
using System.Windows;
using System.Windows.Media;

namespace PolyWars.Adapters {
    /// <summary>
    /// Base class for converting resources to and from the server format
    /// </summary>
    public class ResourceAdapter {
        private static int resourceWidth = 15;
        private static int resourceHight = 15;
        private static int resourceVertices = 4; // Square
        private static int resourceLineThickness = 1;
        private static Color resourceColor = Colors.ForestGreen;
        private static Color resourceBorderColor = Colors.Black;

        public static IResource DTOToResource(ResourceDTO dto) {
            IRay ray = new Ray(dto.ID, new Point(dto.CenterX, dto.CenterY), dto.Angle);
            IRenderable renderable = new Renderable(resourceBorderColor, resourceColor,
                resourceLineThickness, resourceWidth, resourceHight, resourceVertices);

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
