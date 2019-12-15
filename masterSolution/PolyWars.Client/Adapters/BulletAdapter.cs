using PolyWars.Api.Model;
using PolyWars.API.Model.Interfaces;
using PolyWars.API.Network.DTO;
using PolyWars.Logic;
using PolyWars.Network;
using PolyWars.ServerClasses;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PolyWars.Adapters {
    /// <summary>
    /// Base class for converting bullets to and from the server format
    /// and adding to and from the arena
    /// </summary>
    class BulletAdapter {
        private static int bulletWidth = 4;
        private static int bulletHight = 4;
        private static int bulletVertices = 40; // circle-ish
        private static int bulletLineThickness = 1;
        private static int bulletLineMaxVelocity = 19;
        private static Color bulletColor = Colors.DarkViolet;
        private static Color bulletBorderColor = Colors.Black;
        public static async Task<ConcurrentDictionary<string, IBullet>> BulletDTOAdapter() {
            List<BulletDTO> bulletDTOs = await NetworkController.GameService.getBulletsAsync();
            return BulletDTOtoIBullet(bulletDTOs);
        }

        public static ConcurrentDictionary<string, IBullet> BulletDTOtoIBullet(List<BulletDTO> bulletDTOs) {
            ConcurrentDictionary<string, IBullet> bullets = new ConcurrentDictionary<string, IBullet>();
            foreach(BulletDTO bullet in bulletDTOs) {
                Bullet newBullet = renderBullet(bullet);

                if(bullets.TryAdd(bullet.ID, newBullet)) {
                    addBulletToCanvas(newBullet);
                }
            }
            return bullets;
        }

        public static Bullet renderBullet(BulletDTO bullet) {
            IRenderable renderable = new Renderable(bulletBorderColor, bulletColor, bulletLineThickness,
                bulletWidth, bulletHight, bulletVertices);
            IShape shape = new Shape(bullet.ID, bullet.Ray, renderable, new RenderStrategy());
            IMoveable bulletShip = new Moveable(bulletLineMaxVelocity - 1, bulletLineMaxVelocity, 0, 0, shape, new MoveStrategy());
            return new Bullet(bullet.ID, bulletShip, bullet.Damage, bullet.PlayerID);
        }

        public static void removeBulletFromCanvas(string bulletID) {
            if(GameController.Bullets != null && GameController.Bullets.ContainsKey(bulletID)) {
                UIDispatcher.Invoke(() => {
                    IBullet b;
                    while(!GameController.Bullets.TryRemove(bulletID, out b)) { Task.Delay(1); }
                    ArenaController.ArenaCanvas.Children.Remove(b.BulletShip.Shape.Polygon);
                });
            }
        }

        public static void addBulletToCanvas(Bullet bullet) {
            if(GameController.Bullets != null && !GameController.Bullets.ContainsKey(bullet.ID)) {
                bullet.BulletShip.Shape.Renderer.Render(bullet.BulletShip.Shape.Renderable, bullet.BulletShip.Shape.Ray);
                UIDispatcher.Invoke(() => {
                    ArenaController.ArenaCanvas.Children.Add(bullet.BulletShip.Shape.Polygon);
                });
            }
        }

        public static BulletDTO bulletToDTO(IBullet bullet) {
            BulletDTO bulletDTO = new BulletDTO() {
                ID = bullet.ID,
                PlayerID = bullet.PlayerID,
                Ray = (Ray) bullet.BulletShip.Shape.Ray,
                Damage = bullet.Damage
            };
            return bulletDTO;
        }
    }
}
