using PolyWars.Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyWars.API.Network.DTO {
    public class BulletDTO {
        public string ID { get; set; }
        public Ray Ray { get; set; }
        public int Damage { get; set; }
        public override string ToString() {
            return ID.ToString() + Damage.ToString();
        }
    }

}

