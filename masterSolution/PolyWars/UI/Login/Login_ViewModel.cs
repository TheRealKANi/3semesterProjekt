using PolyWars.API.Network;
using PolyWars.API.Network.DTO;
using PolyWars.Logic;
using PolyWars.Model;
using PolyWars.Network;
using System.Collections.Generic;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PolyWars.UI.Login {
    class Login_ViewModel : Observable {
        public Login_ViewModel() {
            Urls = new string[] {
                "localhost", // Lan Client Test
                "109.57.212.47", // WAN Client Test
                "polywars.servegame.com"
            };
            ConnectingDialogVisibility = Visibility.Collapsed;
        }
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
        private string[] urls;
        public string[] Urls {
            get {
                return urls;
            }
            set {
                urls = value;
                SelectedUrl = urls[0];
                NotifyPropertyChanged();
            }
        }
        private string selectedUrl;
        public string SelectedUrl {
            get {
                return selectedUrl;
            }
            set {
                selectedUrl = value;
                NetworkController.GameService.ServerIP = value;
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
                NetworkController.IsConnected = await NetworkController.GameService.ConnectAsync();
                user = await NetworkController.GameService.LoginAsync(username, hashedPassword);
                NavigationController.Instance.navigate(Pages.MainMenu);
                GameController.Username = user.Name;
                GameController.UserID = user.ID;
                //NetworkController.GameService.test();
            }
            return user != null ? true : false;
        }
    }
}
