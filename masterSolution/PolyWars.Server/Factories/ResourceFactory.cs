using PolyWars.Api.Model;
using PolyWars.API.Network.DTO;
using System;
using System.Collections.Generic;
using System.Windows;

namespace PolyWars.Server.Factories {
    public static class ResourceFactory {
        private static int id;
        private static Random r;
        private static int margin;
        private static int width;
        private static int height;

        static ResourceFactory() {
            id = 0;
            r = new Random();
            margin = 25;
            height = 500;
            width = 500;
        }
        private static string getId() {
            return (id++).ToString();
        }
        public static IEnumerable<ResourceDTO> generateResources(int amount) {
            return generateResources(amount, 5);
        }
        public static IEnumerable<ResourceDTO> generateResources(int amount, int value) {
            List<ResourceDTO> list = new List<ResourceDTO>();
            for(int i = 0; i < amount; i++) {
                list.Add(generateResource(value));
            }
            return list;
        }
        public static ResourceDTO generateResource() {
            return generateResource(5);
        }
        public static ResourceDTO generateResource(int value) {
            return new ResourceDTO {
                ID = getId(),
                Ray = new Ray(id.ToString(), new Point(r.Next(margin, width), r.Next(margin, height)), r.Next(0, 360)),
                Value = value
            };

        }
    }
}