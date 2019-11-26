using PolyWars.Logic;
using PolyWars.Model;
using System;
using System.Windows.Input;

namespace PolyWars.UI.MainMenu {
    /// <summary>
    /// This class provides navigation in the main menu.
    /// </summary>
    class MainMenu_ViewModel : Observable {
        private ICommand startGame_Command;
        public ICommand StartGame_Command {
            get {
                if( startGame_Command == null ) {
                    startGame_Command = new RelayCommand( ( o ) => { return true; }, ( o ) => { NavigationController.Instance.navigate( NavigationController.Instance.ArenaPage ); } );
                }
                return startGame_Command;
            }
        }
        private ICommand settings_Command;
        public ICommand Settings_Command {
            get {
                if( settings_Command == null ) {
                    settings_Command = new RelayCommand( ( o ) => { return false; }, ( o ) => { throw new NotImplementedException(); } );
                }
                return settings_Command;
            }
        }
        private ICommand login_Command;
        public ICommand Login_Command {
            get {
                if( login_Command == null ) {
                    login_Command = new RelayCommand( ( o ) => { return false; }, ( o ) => { throw new NotImplementedException(); } );
                }
                return login_Command;
            }
        }

        public GameController GameController { get; set; }
    }
}
