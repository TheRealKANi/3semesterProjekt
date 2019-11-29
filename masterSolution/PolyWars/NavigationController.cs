using PolyWars.Model;
using PolyWars.UI.GameArena;
using PolyWars.UI.Login;
using PolyWars.UI.MainMenu;
using System;
using System.Windows.Controls;

namespace PolyWars {
    enum Pages {
        MainMenu,
        Arena,
        Settings,
        Login
    }
    /// <summary>
    /// This class is to navigate the user in the program.
    /// </summary>
    class NavigationController : Observable {
        private static NavigationController instance;
        public static NavigationController Instance {
            get {
                if( instance == null ) {
                    instance = new NavigationController();
                    instance.navigate( Pages.MainMenu);
                }
                return instance;
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
        private Login login;
        public Login Login {
            get {
                if(login == null) {
                    login = new Login();
                }
                return login;
            }
        }

        public void navigate( Pages p ) {
            switch(p) {
                case Pages.MainMenu:
                Frame.Navigate(MainMenu);
                break;
                case Pages.Arena:
                Frame.NavigationService.Navigate(ArenaPage);
                break;
                case Pages.Login:
                Frame.NavigationService.Navigate(Login);
                break;
                default:
                throw new NotImplementedException(); //TODO Settings
            }
        }
    }
}
