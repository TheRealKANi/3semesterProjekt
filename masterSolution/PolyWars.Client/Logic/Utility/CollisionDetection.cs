using PolyWars.Adapters;
using PolyWars.API.Model.Interfaces;
using PolyWars.Client.Model;
using PolyWars.Network;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolyWars.Client.Logic.Utility {
    class CollisionDetection {
        private static TaskFactory taskFactory;

        static CollisionDetection() {
            taskFactory = new TaskFactory();
        }
        /// <summary>
        /// Checks for rough collissions on a entity and then runs a intersects detection 
        /// </summary>
        public static void runCollisionDetection() {
            if(GameController.DebugFrameTimings) {
                FrameDebugTimer.startCollisionTimer();
            }
            List<Task> subTaskList = new List<Task>();
            List<Task> mainTaskList = new List<Task>();
            RoughCollition roughCollitionDetector = new RoughCollition();

            checkResourceCollition(subTaskList, mainTaskList, roughCollitionDetector);
            checkBulletCollition(subTaskList, mainTaskList, roughCollitionDetector);

            Task.WaitAll(subTaskList.ToArray());
            if(GameController.DebugFrameTimings) {
                FrameDebugTimer.stopCollisionTimer();
            }
        }

        private static void checkBulletCollition(List<Task> mainTaskList, List<Task> subTaskList, RoughCollition roughCollitionDetector) {
            if(GameController.Bullets != null) {
                ConcurrentBag<IBullet> collidedWithBullet = new ConcurrentBag<IBullet>();
                checkRoughBulletPlayerColission(roughCollitionDetector, collidedWithBullet, subTaskList);
                parseRoughBulletPlayerColission(mainTaskList, collidedWithBullet, subTaskList);
            }
        }

        private static void checkResourceCollition(List<Task> mainTaskList, List<Task> subTaskList, RoughCollition roughCollitionDetector) {
            if(GameController.Resources != null) {
                ConcurrentBag<IResource> collidedWithResource = new ConcurrentBag<IResource>();
                checkRoughPlayerResourceColission(roughCollitionDetector, collidedWithResource, subTaskList);
                parseRoughPlayerResourceColission(mainTaskList, collidedWithResource, subTaskList);
            }
        }

        private static void parseRoughBulletPlayerColission(List<Task> mainTaskList, ConcurrentBag<IBullet> collidedWithBullet, List<Task> subTaskList) {
            subTaskList.Add(Task.Factory.StartNew(() => {
                while(!collidedWithBullet.IsEmpty) {
                    if(collidedWithBullet.TryTake(out IBullet bullet)) {
                        mainTaskList.Add(taskFactory.StartNew(async () => {
                            bool result = await NetworkController.GameService.playerGotShot(BulletAdapter.bulletToDTO(bullet));
                            if(result) {
                                if(GameController.Bullets != null) {
                                    BulletAdapter.removeBulletFromCanvas(bullet.ID);
                                }
                            }
                        }));
                    } else {
                        collidedWithBullet.Add(bullet);
                    }
                }
            }));
            Task.WaitAll(subTaskList.ToArray());
            subTaskList.Clear();
        }

        private static void checkRoughBulletPlayerColission(RoughCollition roughCollitionDetector, ConcurrentBag<IBullet> collidedWithBullet, List<Task> subTaskList) {
            IEnumerable<IBullet> roughBulletCollitions = roughCollitionDetector.checkCollision(GameController.Player.PlayerShip.Shape, GameController.Bullets.Values.Where(x => x.ID == GameController.Username), (b) => { return b.BulletShip.Shape; });
            foreach(IBullet bullet in roughBulletCollitions) {
                subTaskList.Add(Task.Factory.StartNew(() => {
                    UIDispatcher.Invoke(() => {
                        if(bullet.PlayerID != GameController.Player.Name && GameController.Player.PlayerShip.Shape.Polygon.RenderedGeometry.Bounds.IntersectsWith(bullet.BulletShip.Shape.Polygon.RenderedGeometry.Bounds)) {
                            collidedWithBullet.Add(bullet);
                        }
                    });
                }));
            }
            Task.WaitAll(subTaskList.ToArray());
            subTaskList.Clear();
        }

        private static void parseRoughPlayerResourceColission(List<Task> mainTaskList, ConcurrentBag<IResource> collidedWithResource, List<Task> subTaskList) {
            subTaskList.Add(Task.Factory.StartNew(() => {
                while(!collidedWithResource.IsEmpty) {
                    if(collidedWithResource.TryTake(out IResource resource)) {
                        mainTaskList.Add(taskFactory.StartNew(async () => {
                            bool result = await NetworkController.GameService.playerCollectedResource(resource);
                            if(result) {
                                if(GameController.Resources != null) {
                                    System.Windows.Shapes.Polygon p = GameController.Resources[resource.ID].Shape.Polygon;
                                    UIDispatcher.Invoke(() => {
                                        ArenaController.ArenaCanvas.Children.Remove(p);
                                    });
                                    if(!GameController.Resources.TryRemove(resource.ID, out IResource res)) {
                                        collidedWithResource.Add(resource);
                                    }

                                }
                            }
                        }));
                    }
                }
            }));
            Task.WaitAll(subTaskList.ToArray());
            subTaskList.Clear();
        }

        ///
        private static void checkRoughPlayerResourceColission(RoughCollition roughCollitionDetector, ConcurrentBag<IResource> collidedWithResource, List<Task> subTaskList) {
            IEnumerable<IResource> roughResourceCollitions = roughCollitionDetector.checkCollision(GameController.Player.PlayerShip.Shape, GameController.Resources.Values, (r) => { return r.Shape; });
            foreach(IResource resource in roughResourceCollitions) {
                subTaskList.Add(Task.Factory.StartNew(() => {
                    UIDispatcher.Invoke(() => {
                        if(resource.Shape.Polygon.RenderedGeometry.Bounds.IntersectsWith(GameController.Player.PlayerShip.Shape.Polygon.RenderedGeometry.Bounds)) {
                            collidedWithResource.Add(resource);
                        }
                    });
                }));
            }
            Task.WaitAll(subTaskList.ToArray());
            subTaskList.Clear();
        }
    }
}

