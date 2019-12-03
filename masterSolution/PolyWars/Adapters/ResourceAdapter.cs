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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PolyWars.Adapters {
    class ResourceAdapter {
        public static async Task<ConcurrentDictionary<string, IResource>> ResourceDTOAdapter() {
            List<ResourceDTO> resourceDTOs = await NetworkController.GameService.getResourcesAsync();
            Console.WriteLine("Got Resources from server");

            return ResourceDTOtoIResource(resourceDTOs);
        }
        public static ConcurrentDictionary<string, IResource> ResourceDTOtoIResource(List<ResourceDTO> resourceDTOs) {
            ConcurrentDictionary<string, IResource> resources = new ConcurrentDictionary<string, IResource>();
            // TODO Convert ResourceDTO to IResources and implement the call somewhere
            foreach(ResourceDTO resource in resourceDTOs) {
                string resourceID = resource.Ray.ID;
                IRay ray = resource.Ray;
                IRenderStrategy renderStrategy = new RenderStrategy();
                IRenderable renderable = new Renderable(Colors.Black, Colors.ForestGreen, 1, 15, 15, 4);
                IShape shape = new Shape(resourceID, ray, renderable, renderStrategy);
                resources.TryAdd(resourceID, new Resource(resourceID, shape, resource.Value));
            }
            return resources;
        }
    }
}
