using PolyWars.API.Model;
using PolyWars.API.Network.DTO;
using System.Collections.Generic;
using System.Windows;

namespace PolyWars.Server.Factories {
    public static class BulletFactory {
        private static int id;

        static BulletFactory() {
            id = 0;
        }

        private static string getId() {
            return (id++).ToString();
        }

        public static IEnumerable<BulletDTO> generateBullets(PlayerDTO playerDTO) {
            return generateBullets(5, playerDTO);
        }

        public static IEnumerable<BulletDTO> generateBullets(int amount, PlayerDTO playerDTO) {
            return generateBullets(amount, 5, playerDTO);
        }

        public static IEnumerable<BulletDTO> generateBullets(int amount, int damage, PlayerDTO playerDTO) {
            List<BulletDTO> list = new List<BulletDTO>();
            for(int i = 0; i < amount; i++) {
                list.Add(generateBullet(damage, playerDTO));
            }
            return list;
        }

        public static BulletDTO generateBullet(int damage, PlayerDTO playerDTO) {
            return new BulletDTO {
                ID = getId(),
                // TODO Make shots render from tip of player, or at least the forward facing edge..
                Ray = new Ray(id.ToString(), new Point(playerDTO.centerX, playerDTO.centerY), playerDTO.Angle),
                Damage = damage,
                PlayerID = playerDTO.Name
            };
        }
    }
}

