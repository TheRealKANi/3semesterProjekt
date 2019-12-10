using PolyWars.Adapters;
using PolyWars.API.Model.Interfaces;
using PolyWars.Model;
using PolyWars.Network;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;

namespace PolyWars.Logic.Utility {
    class CollisionDetection {
        private static TaskFactory taskFactory;
        static CollisionDetection() {
            taskFactory = new TaskFactory();
        }
        public static void resourceCollisionDetection() {
            // TODO DEBUG - Starts collision Timer
            FrameDebugTimer.startCollisionTimer();
            List<Task> taskList = new List<Task>();
            if(GameController.Resources != null) {
                RoughCollition rc = new RoughCollition();

                ConcurrentBag<IResource> collidedWithResource = new ConcurrentBag<IResource>();
                ConcurrentBag<IBullet> collidedWithBullet = new ConcurrentBag<IBullet>();

                IShape player = GameController.Player.PlayerShip.Shape;
                IEnumerable<IResource> roughResourceCollitions = rc.checkCollision(player, GameController.Resources.Values, (r) => { return r.Shape; });
                foreach(IResource resource in roughResourceCollitions) {
                    UIDispatcher.Invoke(() => {
                        if(resource.Shape.Polygon.RenderedGeometry.Bounds.IntersectsWith(player.Polygon.RenderedGeometry.Bounds)) {
                            collidedWithResource.Add(resource);
                        }
                    });
                }
                IEnumerable<IBullet> roughBulletCollitions = rc.checkCollision(player, GameController.Bullets.Values, (b) => { return b.BulletShip.Shape; });
                List<Task> tl = new List<Task>();
                foreach(IBullet bullet in roughBulletCollitions) {
                    tl.Add(Task.Factory.StartNew(() => {
                        UIDispatcher.Invoke(() => {
                            if(bullet.PlayerID != GameController.Player.Name && player.Polygon.RenderedGeometry.Bounds.IntersectsWith(bullet.BulletShip.Shape.Polygon.RenderedGeometry.Bounds)) {
                                collidedWithBullet.Add(bullet);
                            }
                        });
                    }));
                }
                Task.WaitAll(tl.ToArray());
                tl.Clear();
                tl.Add(Task.Factory.StartNew(() => {
                    while(!collidedWithResource.IsEmpty) {
                        if(collidedWithResource.TryTake(out IResource resource)) {
                            taskList.Add(taskFactory.StartNew(async () => {
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
                tl.Add(Task.Factory.StartNew(() => {
                    while(!collidedWithBullet.IsEmpty) {
                        if(collidedWithBullet.TryTake(out IBullet bullet)) {
                            taskList.Add(taskFactory.StartNew(async () => {
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
                    // TODO DEBUG - Stops collision Timer
                }));
                Task.WaitAll(taskList.ToArray());
                FrameDebugTimer.stopCollisionTimer();
            }
        }
    }
}

