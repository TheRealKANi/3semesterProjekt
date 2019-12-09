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
    class BulletAdapter {
        public static async Task<ConcurrentDictionary<string, IBullet>> BulletDTOAdapter() {
            List<BulletDTO> bulletDTOs = await NetworkController.GameService.getBulletsAsync();
            //Console.WriteLine("Client - Got Resources from server");
            return BulletDTOtoIBullet(bulletDTOs);
        }

        public static ConcurrentDictionary<string, IBullet> BulletDTOtoIBullet(List<BulletDTO> bulletDTOs) {
            ConcurrentDictionary<string, IBullet> bullets = new ConcurrentDictionary<string, IBullet>();
            // TODO Convert ResourceDTO to IResources and implement the call somewhere
            foreach(BulletDTO bullet in bulletDTOs) {
                Bullet newBullet = renderBullet(bullet);

                if(bullets.TryAdd(bullet.ID, newBullet)) {
                    addBulletToCanvas(newBullet);
                }
            }
            return bullets;
        }

        public static Bullet renderBullet(BulletDTO bullet) {
            IRenderable renderable = new Renderable(Brushes.Black.Color, Brushes.DarkViolet.Color, 1, 4, 4, 40);
            IShape shape = new Shape(bullet.ID, bullet.Ray, renderable, new RenderStrategy());
            IMoveable bulletShip = new Moveable(9, 9, 0, 0, shape, new MoveStrategy());
            return new Bullet(bullet.ID, bulletShip, bullet.Damage);
        }

        //public static void removeBulletFromCanvas(string bulletID) {
        //    if(GameController.Bullets != null && GameController.Bullets.ContainsKey(bulletID)) {
        //        ThreadController.MainThreadDispatcher.Invoke(() => {
        //            if(GameController.Bullets.TryRemove(bulletID, out IBullet b)) {
        //                ArenaController.ArenaCanvas.Children.Remove(b.Shape.Polygon);
        //                //Debug.WriteLine($"Client - Removed Resource: {resourceID} from canvas");
        //            }
        //        });
        //    }
        //}

        public static void addBulletToCanvas(Bullet bullet) {
            bullet.BulletShip.Shape.Renderer.Render(bullet.BulletShip.Shape.Renderable, bullet.BulletShip.Shape.Ray);
            UIDispatcher.Invoke(() => {
                ArenaController.ArenaCanvas.Children.Add(bullet.BulletShip.Shape.Polygon);
            });
        }
    }
}
