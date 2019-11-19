using PolyWars.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyWars.ViewModel {
    class GameArena_ViewModel {
        private GameController gameController;

        public GameArena_ViewModel() {
            gameController = new GameController();
            gameController.prepareGame();
            gameController.playGame();
        }
    }
}
