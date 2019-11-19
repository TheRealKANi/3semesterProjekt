using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyWars.Logic {
    class GameController {
        private InputController inputController;
        private bool isLoaded;

        GameController() {
            inputController = new InputController();
            isLoaded = false;
        }

        public bool isPrepared() {
            return isLoaded;
        }

        public void prepareGame() {
            // TODO  load resources
            // TODO  Init arena
            // TODO  Init sound
            inputController.initInput();
            isLoaded = true;
        }

        public void playGame(Action msgTest)
        {
            msgTest.Invoke();
        }
    }
}
