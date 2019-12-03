using PolyWars.API;
using PolyWars.API.Model.Interfaces;
using PolyWars.Network;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Linq;
using System.Diagnostics;

namespace PolyWars.Logic.Utility {
    class CollisionDetection {
        private static TaskFactory taskFactory;
        static CollisionDetection() {
            taskFactory = new TaskFactory();
        }
        public static async void resourceCollisionDetection() {
            // TODO DEBUG - Starts collision Timer
            if(GameController.Resources != null) {
                FrameDebugTimer.startCollisionTimer();

                ConcurrentBag<IResource> collidedWith = new ConcurrentBag<IResource>();
                foreach(IResource resource in GameController.Resources) {
                    if(resource.Shape.Polygon.RenderedGeometry.Bounds.IntersectsWith(GameController.Player.PlayerShip.Shape.Polygon.RenderedGeometry.Bounds)) {
                        collidedWith.Add(resource);
                    }
                }
                List<Task> taskList = new List<Task>();
                foreach(IResource resource in collidedWith) {
                    taskFactory.StartNew(async () => {
                        bool result = await NetworkController.GameService.playerCollectedResource(resource);

                        if(result) {
                            ThreadController.MainThreadDispatcher.Invoke(() => {
                                ArenaController.ArenaCanvas.Children.Remove(resource.Shape.Polygon);
                            });
                            GameController.Resources.Remove(resource);
                            Debug.WriteLine($"Removed Resource {resource.ID}");
                        } else {
                            return;
                        }
                    });
                }
                // TODO DEBUG - Stops collision Timer
                FrameDebugTimer.stopCollisionTimer();
            }
        }
    }
}

