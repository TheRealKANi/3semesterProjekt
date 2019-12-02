﻿using PolyWars.API.Network;
using PolyWars.Model;
using PolyWars.Network;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PolyWars.UI.Login {
    class Login_ViewModel : Observable {
        private IUser user;
        private string name;
        public string Name {
            get {
                return name;
            }
            set {
                name = value;
                NotifyPropertyChanged();
            }
        }
        private string hashedPassword;
        public string HashedPassword {
            get {
                return hashedPassword;
            }
            set {
                hashedPassword = value;
                NotifyPropertyChanged();
            }
        }

        private ICommand loginCommand;
        public ICommand LoginCommand {
            get {

                return loginCommand ?? (loginCommand = new RelayCommandAsync(() => Login(Name, HashedPassword)));
            }
        }


        private async Task<bool> Login(string username, string hashedPassword) {
            if(!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(hashedPassword)) {
                await NetworkController.GameService.ConnectAsync();
                user = await NetworkController.GameService.LoginAsync(username, hashedPassword);
                NavigationController.Instance.navigate(Pages.MainMenu);
            }
            return user != null ? true : false;
        }
    }
}