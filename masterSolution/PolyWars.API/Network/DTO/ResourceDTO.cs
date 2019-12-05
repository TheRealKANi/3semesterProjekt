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
        public Ray Ray { get; set; }
        public int Value { get; set; }
        public override string ToString() {
            return ID.ToString() + Value.ToString();
        }
    }
}
