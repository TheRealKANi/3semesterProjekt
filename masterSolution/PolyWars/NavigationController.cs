using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using PolyWars.UI.MainMenu;
using PolyWars.UI.GameArena;
using PolyWars.Model;

namespace PolyWars {
    /// <summary>
    /// This class is to navigate the user in the program.
    /// </summary>
    class NavigationController : Observable {
        private static NavigationController instance;
        public static NavigationController Instance {
            get {
                if(instance == null) {
                    instance = new NavigationController();
                    instance.Navigate(instance.MainMenu);
                }
                return instance;
            }
        }

        private Frame frame;
        public Frame Frame {
            get {
                if(frame == null) {
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
                if(mainMenu == null) {
                    mainMenu = new MainMenu();
                }
                return mainMenu;
            }
        }

        private GameArenaPage arenaPage;
        public GameArenaPage ArenaPage { 
            get {
                if(arenaPage == null) {
                    arenaPage = new GameArenaPage();
                }
                return arenaPage;
            }
            set {
                arenaPage = value;
            }
        }

        public void Navigate(Page p) {
            Frame.NavigationService.Navigate(p);
        }
    }
}
