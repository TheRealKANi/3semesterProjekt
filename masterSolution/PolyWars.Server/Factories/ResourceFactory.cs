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
            margin = 50;
            height = 768;
            width = 1024;
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
                CenterX = r.Next(margin/2, width-margin), 
                CenterY = r.Next(margin/2, height-(margin*3)), 
                Angle = r.Next(0, 360),
                Value = value
            };

        }
    }
}