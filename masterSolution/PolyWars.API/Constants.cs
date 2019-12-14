using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyWars.API {
    public static class Constants {
        public static readonly string protocol = "http://";
        public static readonly string serverPort = "5700";
        public static readonly string serverEndPoint = "/GameService";
        public static readonly int standardShotDamage = 20;

        public static readonly string servicePort = "5701";
        public static readonly string serviceEndPoint = "/WebClientService";

    }
}
