using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PolyWars.Logic
{
    class GameController
    {
        private InputController inputController;
        private bool isLoaded;

        public GameController() {
            inputController = new InputController();
            isLoaded = false;
        }

        public Canvas prepareGame() {
            return new Canvas(); //TODO will "probably" need more code eventually
        }

        public void playGame() {
            //throw new NotImplementedException();
        }

        public bool isPrepared() {
            return isLoaded;
        }
    }
}