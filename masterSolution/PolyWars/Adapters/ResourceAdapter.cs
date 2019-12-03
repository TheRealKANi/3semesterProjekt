using PolyWars.API.Model.Interfaces;
using PolyWars.API.Network.DTO;
using PolyWars.API.Strategies;
using PolyWars.Logic;
using PolyWars.Network;
using PolyWars.Server.Model;
using PolyWars.ServerClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PolyWars.Adapters {
    class ResourceAdapter {
        public static async Task<List<IResource>> ResourceDTOAdapter() {
            List<ResourceDTO> resourceDTOs = await NetworkController.GameService.getResourcesAsync();

            List<IResource> resources = new List<IResource>();
            // TODO Convert ResourceDTO to IResources and implement the call somewhere
            foreach(ResourceDTO resource in resourceDTOs) {
                string resourceID = resource.Ray.ID;
                IRay ray = resource.Ray;
                IRenderStrategy renderStrategy = new RenderStrategy();
                IRenderable renderable = new Renderable(Colors.Black, Colors.ForestGreen, 1, 15, 15, 4);
                IShape shape = new Shape(resourceID, ray, renderable, renderStrategy);
                resources.Add(new Resource(resourceID, shape, resource.Value));
            }
            return resources;
        }
    }
}
