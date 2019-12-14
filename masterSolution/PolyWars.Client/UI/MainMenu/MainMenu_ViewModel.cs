﻿using PolyWars.Logic;
using PolyWars.Model;
using PolyWars.Network;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PolyWars.UI.MainMenu {
    /// <summary>
    /// This class provides navigation in the main menu.
    /// </summary>
    class MainMenu_ViewModel : Observable {
        private bool isConnected;
        public bool IsConnected {
            get {
                return isConnected;
            }
            set {
                isConnected = value;
                NotifyPropertyChanged();
            }
        }
        public bool IsLoggedIn {
            get {
                return isLoggedIn;
            }
            set {
                isLoggedIn = value;
                NotifyPropertyChanged();
            }
        }
        public ICommand StartGame_Command {
            get {
                if(startGame_Command == null) {
                    startGame_Command = new RelayCommand((o) => { return NetworkController.IsConnected; }, startGame);
                }
                return startGame_Command;
            }
        }
        private ICommand settings_Command;
        public ICommand Settings_Command {
            get {
                if(settings_Command == null) {
                    settings_Command = new RelayCommand((o) => { return false; }, (o) => { throw new NotImplementedException(); });
                }
                return settings_Command;
            }
        }
        public ICommand Login_Command {
            get {
                return login_Command ?? (login_Command = new RelayCommand((o) => { return true; }, (o) => NavigationController.Instance.navigate(Pages.Login)));
            }
        }
            NetworkController.GameService.initIngameBindings();
    }
}