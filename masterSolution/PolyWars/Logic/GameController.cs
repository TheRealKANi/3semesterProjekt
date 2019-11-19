using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyWars.Logic
{
    class GameController
    {
        public static void PlayGame(Action msgTest) {
            msgTest.Invoke();
        }
    }
}
