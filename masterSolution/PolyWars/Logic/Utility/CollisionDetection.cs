using PolyWars.API.Model.Interfaces;
using PolyWars.Network;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

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

                foreach(IResource resource in GameController.Resources.Values) {
                    ThreadController.MainThreadDispatcher.Invoke(() => {
                        if(resource.Shape.Polygon.RenderedGeometry.Bounds.IntersectsWith(GameController.Player.PlayerShip.Shape.Polygon.RenderedGeometry.Bounds)) {
                            collidedWith.Add(resource);
                        }
                    });
                }
                List<Task> taskList = new List<Task>();
                foreach(IResource resource in collidedWith) {
                    taskList.Add(taskFactory.StartNew(async () => {
                        bool result = await NetworkController.GameService.playerCollectedResource(resource);
                        if(result) {
                            ThreadController.MainThreadDispatcher.Invoke(() => {
                                if(GameController.Resources != null) {
                                    System.Windows.Shapes.Polygon p = GameController.Resources[resource.ID].Shape.Polygon;
                                    ArenaController.ArenaCanvas.Children.Remove(p);
                                }
                            });
                        }
                    }));
                }
                Task.WaitAll(taskList.ToArray());
                // TODO DEBUG - Stops collision Timer
                FrameDebugTimer.stopCollisionTimer();
            }
        }
    }
}

