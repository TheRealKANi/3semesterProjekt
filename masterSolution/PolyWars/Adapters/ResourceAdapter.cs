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
using System.Windows.Media;

namespace PolyWars.Adapters {
    class ResourceAdapter {
        public static async Task<ConcurrentDictionary<string, IResource>> ResourceDTOAdapter() {
            List<ResourceDTO> resourceDTOs = await NetworkController.GameService.getResourcesAsync();
            //Console.WriteLine("Client - Got Resources from server");
            return ResourceDTOtoIResource(resourceDTOs);
        }

        public static ConcurrentDictionary<string, IResource> ResourceDTOtoIResource(List<ResourceDTO> resourceDTOs) {
            ConcurrentDictionary<string, IResource> resources = new ConcurrentDictionary<string, IResource>();
            // TODO Convert ResourceDTO to IResources and implement the call somewhere
            foreach(ResourceDTO resource in resourceDTOs) {
                IRay ray = resource.Ray;
                IRenderStrategy renderStrategy = new RenderStrategy();
                IRenderable renderable = new Renderable(Colors.Black, Colors.ForestGreen, 1, 15, 15, 4);
                IShape shape = new Shape(resource.ID, ray, renderable, renderStrategy);
                Resource newResource = new Resource(resource.ID, shape, resource.Value);
                if(resources.TryAdd(resource.ID, newResource)) {
                    addResourceToCanvas(newResource);
                }
            }
            return resources;
        }

        public static void removeResourceFromCanvas(string resourceID) {
            if(GameController.Resources != null && GameController.Resources.ContainsKey(resourceID)) {
                ThreadController.MainThreadDispatcher.Invoke(() => {
                    if(GameController.Resources.TryRemove(resourceID, out IResource r)) {
                        ArenaController.ArenaCanvas.Children.Remove(r.Shape.Polygon);
                        //Debug.WriteLine($"Client - Removed Resource: {resourceID} from canvas");
                    }
                });
            }
        }

        public static void addResourceToCanvas(Resource resource) {
            resource.Shape.Renderer.Render(resource.Shape.Renderable, resource.Shape.Ray);
            ThreadController.MainThreadDispatcher.Invoke(() => {
                ArenaController.ArenaCanvas.Children.Add(resource.Shape.Polygon);
            });
        }
    }
}
