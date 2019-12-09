using PolyWars.API.Model.Interfaces;
using PolyWars.Network;
using PolyWars.Server.Model;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PolyWars.Logic.Utility {
    class CollisionDetection {
        private static TaskFactory taskFactory;
        private static bool isActive;
        static CollisionDetection() {
            taskFactory = new TaskFactory();
            isActive = false;
        }
        public static async void resourceCollisionDetection() {
            // TODO DEBUG - Starts collision Timer
            if(!isActive) {
                isActive = true;
                List<Task> taskList = new List<Task>();
                if(GameController.Resources != null) {
                    FrameDebugTimer.startCollisionTimer();

                    ConcurrentBag<IResource> collidedWith = new ConcurrentBag<IResource>();

                    foreach(IResource resource in GameController.Resources.Values) {
                        UIDispatcher.Invoke(() => {
                            if(resource.Shape.Polygon.RenderedGeometry.Bounds.IntersectsWith(GameController.Player.PlayerShip.Shape.Polygon.RenderedGeometry.Bounds)) {
                                collidedWith.Add(resource);
                            }
                        });
                    }
                    while(!collidedWith.IsEmpty) {
                        if(collidedWith.TryTake(out IResource resource)) {
                            taskList.Add(taskFactory.StartNew(async () => {
                                bool result = await NetworkController.GameService.playerCollectedResource(resource);
                                if(result) {
                                    if(GameController.Resources != null) {
                                        System.Windows.Shapes.Polygon p = GameController.Resources[resource.ID].Shape.Polygon;
                                        UIDispatcher.Invoke(() => {
                                            ArenaController.ArenaCanvas.Children.Remove(p);
                                        });
                                        if(!GameController.Resources.TryRemove(resource.ID, out IResource res)) {
                                            collidedWith.Add(resource);
                                        }

                                    }
                                }
                            }));
                        }
                    }
                    // TODO DEBUG - Stops collision Timer
                    FrameDebugTimer.stopCollisionTimer();
                }
                await Task.Factory.StartNew(() => Task.WaitAll(taskList.ToArray()));
                isActive = false;
            }
        }
    }
}

