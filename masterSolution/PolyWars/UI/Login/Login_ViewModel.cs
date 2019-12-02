using PolyWars.Model;
using PolyWars.Network;
using System.Security;
using System.Windows.Input;

namespace PolyWars.UI.Login {
    class Login_ViewModel : Observable {
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
        private SecureString password;
        public SecureString Password {
            get {
                return password;
            }
            set {
                password = value;
                NotifyPropertyChanged();
            }
        }
        private ICommand loginCommand;
        public ICommand LoginCommand {
            get {
                if(loginCommand == null) {
                    loginCommand = new RelayCommand((o) => {
                        return !string.IsNullOrWhiteSpace(Name);
                    }, Login);
                }
                return loginCommand;
            }
        }
        private void Login(object o) {
            NetworkController.GameService.ConnectAsync().Wait();
            NetworkController.GameService.LoginAsync(Name).Wait();
            NavigationController.Instance.navigate(Pages.MainMenu);
        }
    }
}
