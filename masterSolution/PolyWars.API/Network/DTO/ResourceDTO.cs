using PolyWars.Api.Model;
using PolyWars.API.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyWars.API.Network.DTO {
    public class ResourceDTO {

        public string ID { get; set; }
        public double Value { get; set; }
        public double CenterX { get; set; }
        public double CenterY { get; set; }
        public double Angle { get; set; }
        public override string ToString() {
            return ID.ToString() + Value.ToString();
        }
    }
}
