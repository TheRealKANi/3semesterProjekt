using PolyWars.Logic;
using PolyWars.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PolyWars.UI.StartPage
{
    class StartPage_ViewModel : INotifyPropertyChanged
    {
        private ICommand startGame_Command;
        public ICommand StartGame_Command
        {
            get {
                if(startGame_Command == null) {
                    startGame_Command = new RelayCommand((o) => { return true; }, (o) => { GameController.PlayGame(() => { MessageBox.Show("The Button Works!"); }); });
                }
                return startGame_Command;
            }
        }
        private ICommand settings_Command;
        public ICommand Settings_Command
        {
            get {
                if(settings_Command == null) {
                    settings_Command = new RelayCommand((o) => { return false; }, (o) => { throw new NotImplementedException(); });
                }
                return settings_Command;
            }
        }
        private ICommand login_Command;
        public ICommand Login_Command
        {
            get {
                if(login_Command == null) {
                    login_Command = new RelayCommand((o) => { return false; }, (o) => { throw new NotImplementedException(); });
                }
                return login_Command;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
