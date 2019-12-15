using PolyWars.API;
using PolyWars.API.Network;
using PolyWars.Client.Logic;
using PolyWars.Client.Model;
using PolyWars.Network;
using System;
using System.Threading.Tasks;
using System.Windows;using System.Windows.Controls;
using System.Windows.Input;

namespace PolyWars.Client.UI.Login {
    class Login_ViewModel : Observable {        public Login_ViewModel() {
            Urls = Constants.serverList;            ConnectingDialogVisibility = Visibility.Collapsed;        }
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
        private string[] urls;
        public string[] Urls {            get {                return urls;            }            set {                urls = value;                SelectedUrl = urls[0];                NotifyPropertyChanged();            }        }        private string selectedUrl;        public string SelectedUrl {            get {                return selectedUrl;            }            set {                selectedUrl = value;                NetworkController.GameService.ServerIP = value;                NotifyPropertyChanged();            }        }        private string connectingDialogText;        public string ConnectingDialogText {            get {                return connectingDialogText;            }            set {                connectingDialogText = "Connecting to: " + value;                NotifyPropertyChanged();            }        }        private Visibility connectingDialogVisibility;        public Visibility ConnectingDialogVisibility {            get {                return connectingDialogVisibility;            }            set {                connectingDialogVisibility = value;                NotifyPropertyChanged();            }        }        private ICommand loginCommand;
        public ICommand LoginCommand {
            get {
                return loginCommand ?? (loginCommand = new RelayCommandAsync((o) => Login(Name, (o as PasswordBox).Password)));
            }
        }
        private async Task<bool> Login(string username, string hashedPassword) {            if(!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(hashedPassword)) {                await Task.Run(() => {                    ConnectingDialogText = "Connecting to: " + SelectedUrl;                    ConnectingDialogVisibility = Visibility.Visible;                    UIDispatcher.Invoke(() => NavigationController.Instance.Login.IsEnabled = false);
                });                NetworkController.IsConnected = await NetworkController.GameService.ConnectAsync();                await Task.Run(() => {                    ConnectingDialogText = "Logging in";                    ConnectingDialogVisibility = Visibility.Collapsed;                    UIDispatcher.Invoke(() => NavigationController.Instance.Login.IsEnabled = true);
                });
                try {
                    user = await NetworkController.GameService.LoginAsync(username, hashedPassword);
                    NavigationController.Instance.navigate(Pages.MainMenu);
                    GameController.Username = user.Name;
                    GameController.UserID = user.ID;
                } catch(NullReferenceException) {
                    UIDispatcher.Invoke(() => MessageBox.Show("Invalid Login information"));
                }
            }
            return user != null ? true : false;
        }
    }
}
