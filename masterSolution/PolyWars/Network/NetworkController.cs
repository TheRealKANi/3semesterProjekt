using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyWars.Network {
    static class NetworkController {
        static NetworkController() {
            GameService = new GameService();
        }
        public static GameService GameService { get; private set; }
    }
}
