using PolyWars.Logic;
using PolyWars.Model;
using System;
using System.Windows.Input;

namespace PolyWars.UI.MainMenu {
    /// <summary>
    /// This class provides navigation in the main menu.
    /// </summary>
    class MainMenu_ViewModel : Observable {
        //private List<IPlayer> Participants;
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
        private bool isLoggedIn;
        public bool IsLoggedIn {
            get {
                return isLoggedIn;
            }
            set {
                isLoggedIn = value;
                NotifyPropertyChanged();
            }
        }
        private ICommand startGame_Command;
        public ICommand StartGame_Command {
            get {
                if(startGame_Command == null) {
                    startGame_Command = new RelayCommand((o) => { return true /*IsConnected && IsLoggedIn;*/; }, (o) => { NavigationController.Instance.navigate(Pages.Arena); });
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
        private ICommand login_Command;
        public ICommand Login_Command {
            get {
                if(login_Command == null) {
                    login_Command = new RelayCommand((o) => { return true; }, (o) => { NavigationController.Instance.navigate(Pages.Login); });
                }
                return login_Command;
            }
        }

        /*private static async Task<bool> Login() {
            try {
                List<IPlayer> players = new List<IPlayer>();
                players = await GameService.LoginAsync(player.Name);
                if(IPlayers != null) {
                    IPlayers.ForEach(u => Participants.Add(new Plyaer() { Name = u.Name, ID = u.ID }));
                    IsLoggedIn = true;
                    return true;
                } else { return false; }

            } catch(Exception) { return false; }
        }
        private static async Task<bool> Logout() {
            try {
                await GameService.LogoutAsync();
                IsLoggedIn = false;
                return true;
            } catch(Exception) { return false; }
        }
        private static async Task<bool> SendTextMessage() {
            try {
                await GameService.SendBroadcastMessageAsync(_textMessage);
                return true;
            } catch(Exception e) {
                Console.WriteLine(e.Message + "\n" + printError(e)); return false;
            }
        }

        private static void ParticipantLogin(IPlayer u) {
            var ptp = Participants.FirstOrDefault(p => string.Equals(p.Name, u.Name));
            if(IsLoggedIn && ptp == null) {
                ctxTaskFactory.StartNew(() => Participants.Add(new IPlayer {
                    Name = u.Name,
                    ID = u.ID
                })).Wait();
            }
        }

        private static void ParticipantDisconnection(string name) {
            var person = Participants.Where((p) => string.Equals(p.Name, name)).FirstOrDefault();
            if(person != null) IsLoggedIn = false;
        }

        private static void ParticipantReconnection(string name) {
            var person = Participants.Where((p) => string.Equals(p.Name, name)).FirstOrDefault();
            if(person != null) IsLoggedIn = true;
        }

        private static void Reconnecting() {
            IsConnected = false;
            IsLoggedIn = false;
        }

        private static async void Reconnected() {
            if(!string.IsNullOrEmpty(IPlayer.Name)) await GameService.LoginAsync(IPlayer.Name);
            IsConnected = true;
            IsLoggedIn = true;
        }

        private static async void Disconnected() {
            var connectionTask = GameService.ConnectAsync();
            await connectionTask.ContinueWith(t => {
                if(!t.IsFaulted) {
                    IsConnected = true;
                    GameService.LoginAsync(IPlayer.Name).Wait();
                    IsLoggedIn = true;
                }
            });
        }
        public static async void NewTextMessage(string author, string message) {
            string msg = author + "\n" + message;
            var sender = Participants.Where((u) => string.Equals(u.Name, author)).FirstOrDefault();

            await ctxTaskFactory.StartNew(() => Console.WriteLine(msg));
        }*/

        public GameController GameController { get; set; }
    }
}
