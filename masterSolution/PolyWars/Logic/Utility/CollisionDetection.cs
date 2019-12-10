using PolyWars.Adapters;
using PolyWars.API.Model.Interfaces;
using PolyWars.Network;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

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

                ConcurrentBag<IResource> collidedWithResource = new ConcurrentBag<IResource>();
                ConcurrentBag<IBullet> collidedWithBullet = new ConcurrentBag<IBullet>();

                foreach(IResource resource in GameController.Resources.Values) {
                    UIDispatcher.Invoke(() => {
                        if(resource.Shape.Polygon.RenderedGeometry.Bounds.IntersectsWith(GameController.Player.PlayerShip.Shape.Polygon.RenderedGeometry.Bounds)) {
                            collidedWithResource.Add(resource);
                        }
                    });
                }
                //foreach(IBullet bullet in GameController.Bullets.Values) {
                //    foreach(IMoveable opponent in GameController.Opponents.Values) {
                //        UIDispatcher.Invoke(() => {
                //            if(bullet.PlayerID != GameController.Player.Name && bullet.BulletShip.Shape.Polygon.RenderedGeometry.Bounds.IntersectsWith(GameController.Player.PlayerShip.Shape.Polygon.RenderedGeometry.Bounds)) {
                //                collidedWithBullet.Add(bullet);
                //            }
                //        });
                //    }
                //}
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
                //while(!collidedWithBullet.IsEmpty) {
                //    if(collidedWithBullet.TryTake(out IBullet bullet)) {
                //        taskList.Add(taskFactory.StartNew(async () => {
                //            bool result = await NetworkController.GameService.playerGotShot(BulletAdapter.bulletToDTO(bullet));
                //            if(result) {
                //                if(GameController.Bullets != null) {
                //                    BulletAdapter.removeBulletFromCanvas(bullet.ID);
                //                }
                //            }
                //        }));
                //    } else {
                //        collidedWithBullet.Add(bullet);
                //    }
                //}
                // TODO DEBUG - Stops collision Timer
            }
            Task.WaitAll(taskList.ToArray());
            FrameDebugTimer.stopCollisionTimer();
        }
    }
}

