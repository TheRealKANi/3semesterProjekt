﻿using PolyWars.Client.Logic;
using PolyWars.Client.Model;
using PolyWars.Network;
using System;
using System.Windows.Input;

namespace PolyWars.Client.UI.MainMenu {
    /// <summary>
    /// This class provides navigation in the main menu.
    /// </summary>
    class MainMenu_ViewModel : Observable {        public GameController GameController { get; set; }        public MainMenu_ViewModel() { }
        private bool isConnected;
        public bool IsConnected {
            get {
                return isConnected;
            }
            set {
                isConnected = value;
                NotifyPropertyChanged();
            }
        }        private bool isLoggedIn;
        public bool IsLoggedIn {
            get {
                return isLoggedIn;
            }
            set {
                isLoggedIn = value;
                NotifyPropertyChanged();
            }
        }        private ICommand startGame_Command;
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
        }        private ICommand login_Command;
        public ICommand Login_Command {
            get {
                return login_Command ?? (login_Command = new RelayCommand((o) => { return true; }, (o) => NavigationController.Instance.navigate(Pages.Login)));
            }
        }        private void startGame() {            GameController gameController = new GameController();            gameController.prepareGame();
            NetworkController.GameService.initIngameBindings();            NavigationController.Instance.navigate(Pages.Arena);            gameController.playGame();        }
    }
}
