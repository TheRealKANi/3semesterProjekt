using PolyWars.UI.StartPage;
using PolyWars.UI.GameArena;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PolyWars {
    /// <summary>
    /// This class is to navigate the user in the program.
    /// </summary>
    class NavigationController : INotifyPropertyChanged {
        private static NavigationController instance;
        public static NavigationController Instance {
            get {
                if(instance == null) {
                    instance = new NavigationController();
                    instance.Navigate(instance.StartPage);
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

        private StartPage startPage;
        public StartPage StartPage { 
            get {
                if(startPage == null) {
                    startPage = new StartPage();
                }
                return startPage;
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

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
