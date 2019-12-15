using PolyWars.Adapters;
using PolyWars.API.Model;
using PolyWars.API.Model.Interfaces;
using PolyWars.API.Network.DTO;
using PolyWars.Client.Logic;
using PolyWars.Client.Model;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace PolyWars.Network {
    static class NetworkController {
        private static bool isConnected;
        public static bool IsConnected {
            get {
                return isConnected;
            }
            set {
                isConnected = value;
                UIDispatcher.Invoke(() => CommandManager.InvalidateRequerySuggested());
            }
        }
        public static GameService GameService { get; private set; }

        static NetworkController() {
            GameService = new GameService();

            GameService.announceClientLoggedIn += announceClientLoggedIn;
            //GameService.accessDenied += deniedAccess;
            //GameService.updateOpponents += updateOpponents;
            //GameService.updateResources += updateResources;
            GameService.removeResource += removeResource;
            GameService.clientLoggedOut += clientLoggedOut;
            GameService.opponentMoved += opponentMoved;
            GameService.updateWallet += updateWallet;
            GameService.opponentJoined += opponentJoined;
            GameService.opponentShoots += opponentShoots;
            GameService.updateHealth += updateHealth;
            GameService.playerDied += playerDied;
            GameService.removeBullet += removeBullet;
            GameService.removeDeadOpponent += removeDeadOpponent;
        }

        private static void removeDeadOpponent(string username) {
            IMoveable deadPlayer = null;
            while(GameController.Opponents != null && !GameController.Opponents.TryRemove(username, out deadPlayer)) {
                Task.Delay(1);
            }
            if(deadPlayer != null) {
                UIDispatcher.Invoke(() => ArenaController.ArenaCanvas.Children.Remove(deadPlayer.Shape.Polygon));
            }
        }

        private static void removeBullet(BulletDTO bullet) {
            BulletAdapter.removeBulletFromCanvas(bullet.ID);
        }

        private static void playerDied(string killedBy) {
            // Remove player from canvas and disable input
            GameController.IsPlayerDead = true;
            GameController.Player.Health = 0;
            UIDispatcher.Invoke(() => ArenaController.ArenaCanvas.Children.Remove(GameController.Player.PlayerShip.Shape.Polygon));
            Debug.WriteLine("Announce: " + GameController.Player.Name + " got killed by " + killedBy);
        }

        private static void updateHealth(int healthLeft) {
            GameController.Player.Health = healthLeft;
        }

        private static void opponentShoots(BulletDTO bullet) {
            Bullet newBullet = BulletAdapter.renderBullet(bullet);
            if(GameController.Bullets.TryAdd(newBullet.ID, newBullet)) {
                BulletAdapter.addBulletToCanvas(newBullet);
            }
        }

        private static void updateWallet(double walletAmount) {
            GameController.Player.Wallet = walletAmount;
        }

        private static void opponentJoined(PlayerDTO dto) {
            if(!GameController.Opponents.ContainsKey(dto.Name)) {
                IMoveable opponent = PlayerAdapter.playerDTOToMoveable(dto, Colors.Red);
                if(GameController.Opponents.TryAdd(dto.Name, opponent)) {
                    UIDispatcher.Invoke(() => ArenaController.ArenaCanvas.Children.Add(opponent.Shape.Polygon));
                }
            }
        }

        private static void opponentMoved(PlayerDTO dto) {
            if(GameController.Opponents.ContainsKey(dto.Name)) {
                IMoveable opponent = GameController.Opponents[dto.Name];
                opponent.Velocity = dto.Velocity;
                opponent.MaxVelocity = dto.MaxVelocity;
                opponent.RPM = dto.RPM;
                opponent.MaxRPM = dto.MaxRPM;

                IRay ray = new Ray(opponent.Shape.Ray.ID, new Point(dto.centerX, dto.centerY), dto.Angle);
                opponent.Shape.Ray = ray;
            }
        }

        private static void clientLoggedOut(string id) {
            if(GameController.Opponents.ContainsKey(id)) {
                IMoveable opponent;
                while(!GameController.Opponents.TryRemove(id, out opponent)) {
                    Task.Delay(5);
                }
                UIDispatcher.Invoke(() => ArenaController.ArenaCanvas.Children.Remove(opponent.Shape.Polygon));
            }
        }

        private static void removeResource(string id) {
            if(GameController.Resources.ContainsKey(id)) {
                IResource resource;
                while(!GameController.Resources.TryRemove(id, out resource)) {
                    Task.Delay(5);
                }
                UIDispatcher.Invoke(() => ArenaController.ArenaCanvas.Children.Remove(resource.Shape.Polygon));
            }
        }

        public static void announceClientLoggedIn(string userName) {
            Debug.WriteLine($"Announce: '{userName}' has joined the lobby");
        }
    }
}
