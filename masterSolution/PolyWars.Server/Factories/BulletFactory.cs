using PolyWars.Api.Model;
using PolyWars.API.Network.DTO;
using System;
using System.Collections.Generic;
using System.Windows;

namespace PolyWars.Server.Factories {
    public static class BulletFactory {
        private static int id;
        private static Random r;
        private static int margin;
        private static int width;
        private static int height;

        static BulletFactory() {
            id = 0;
            r = new Random(5642318);
            margin = 25;
            height = 500;
            width = 500;
        }
        private static string getId() {
            return (id++).ToString();
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
        public static IEnumerable<BulletDTO> generateBullets(PlayerDTO playerDTO) {
            return generateBullets(5, playerDTO);
        }
        public static BulletDTO generateBullet(int damage, PlayerDTO playerDTO) {
            return new BulletDTO {
                ID = getId(),
                Ray = new Ray(id.ToString(), new Point(playerDTO.centerX, playerDTO.centerY), playerDTO.Angle),
                Damage = damage,
                PlayerID = playerDTO.Name
            };

        }
    }
}

