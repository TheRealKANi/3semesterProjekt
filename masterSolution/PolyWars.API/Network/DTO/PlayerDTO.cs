using PolyWars.API.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyWars.API.Network.DTO {
    public class PlayerDTO {
        public string Name { get; set; }
        public string ID { get; private set; }
        public double Wallet { get; set; }
        public int Vertices { get; set; }
        public IRay Ray { get; set; }
    }
}
