using PolyWars.Model;
using PolyWars.UI.GameArena;
using PolyWars.UI.MainMenu;
using System.Windows.Controls;

namespace PolyWars {
    /// <summary>
    /// This class is to navigate the user in the program.
    /// </summary>
    class NavigationController : Observable {
        private static NavigationController INSTANCE;
        public static NavigationController Instance {
            get {
                if( INSTANCE == null ) {
                    INSTANCE = new NavigationController();
                    INSTANCE.navigate( INSTANCE.MainMenu );
                }
                return INSTANCE;
            }
        }

        private Frame frame;
        public Frame Frame {
            get {
                if( frame == null ) {
                    Frame = new Frame();
                }
                return frame;
            }
            set {
                frame = value;
                NotifyPropertyChanged();
            }
        }

        private MainMenu mainMenu;
        public MainMenu MainMenu {
            get {
                if( mainMenu == null ) {
                    mainMenu = new MainMenu();
                }
                return mainMenu;
            }
        }

        private GameArenaPage arenaPage;
        public GameArenaPage ArenaPage {
            get {
                if( arenaPage == null ) {
                    arenaPage = new GameArenaPage();
                }
                return arenaPage;
            }
            set {
                arenaPage = value;
            }
        }

        public void navigate( Page p ) {
            Frame.NavigationService.Navigate( p );
        }
    }
}
