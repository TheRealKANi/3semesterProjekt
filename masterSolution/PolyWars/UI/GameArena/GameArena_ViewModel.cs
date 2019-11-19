using PolyWars.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PolyWars.UI.GameArena
{
    class GameArena_ViewModel
    {
        private GameController gameController;
        public GameArena_ViewModel() {
            gameController = new GameController();
            ArenaCanvas = gameController.prepareGame();
            gameController.playGame();
        }

        private Canvas arenaCanvas;
        public Canvas ArenaCanvas
        {
            get {
                return arenaCanvas;
            }
            set {
                arenaCanvas = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
