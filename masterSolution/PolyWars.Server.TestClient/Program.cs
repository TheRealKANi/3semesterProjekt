using PolyWars.API;
using PolyWars.Server.TestClient.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace PolyWars.Server.TestClient {
    class Program {
        public static GameService GameService;
        public static bool IsConnected { get; set; }
        public static User User { get; set; }
        public static string _textMessage;

        public static List<User> Participants;
        public static bool IsLoggedIn;
        public static List<ILobby> lobbies;


        private static TaskFactory ctxTaskFactory;

        private static void MakeGameService() {
            Participants = new List<User>();
            ctxTaskFactory = new TaskFactory();
            GameService = new GameService();
            GameService.NewTextMessage += NewTextMessage;
            GameService.ParticipantLoggedIn += ParticipantLogin;
            GameService.ParticipantLoggedOut += ParticipantDisconnection;
            GameService.ParticipantDisconnected += ParticipantDisconnection;
            GameService.ParticipantReconnected += ParticipantReconnection;
            GameService.ConnectionReconnecting += Reconnecting;
            GameService.ConnectionReconnected += Reconnected;
            GameService.ConnectionClosed += Disconnected;
        }

        static void Main(string[] args) {
            handler = new ConsoleEventDelegate(ConsoleEventCallback);
            SetConsoleCtrlHandler(handler, true);

            _textMessage = string.Empty;

            Console.WriteLine("Input Username: ");
            string userName = Console.ReadLine();

            Console.WriteLine("Input password: ");
            string password = Console.ReadLine();


            User = new User { Name = userName, HashedPassword = password};

            MakeGameService();
            Connect().Wait();
            if(IsConnected) {
                Console.WriteLine("connected");
                Login().Wait();
            }
            if(IsLoggedIn) {
                Console.WriteLine($"{User.Name} logged in");
            } else {
                Console.WriteLine("Could not login");
            }
            Console.ReadLine();
        }
        private static string printError(Exception e) {
            string m = e.Message + " ";
            if(e.InnerException != null)
                m += printError(e.InnerException);
            return m;
        }
        // 1
        private static async Task<bool> Connect() {
            try {
                await GameService.ConnectAsync();
                IsConnected = true;
                return true;
            } catch(Exception e) { Console.WriteLine(printError(e)); return false; }
        }

        // 2
        private static async Task<bool> Login() {
            try {
                lobbies = new List<ILobby>();
                Console.WriteLine("Trying to get Lobbies from server");
                lobbies = await GameService.LoginAsync(User);
                Console.WriteLine("Got Lobbies from server");
                if(lobbies != null) {
                    IsLoggedIn = true;
                    return true;
                } else { return false; }

            } catch(Exception e) { Console.WriteLine(printError(e)); return false; }
        }
        private static async Task<bool> Logout() {
            try {
                await GameService.LogoutAsync();
                IsLoggedIn = false;
                return true;
            } catch(Exception e) { Console.WriteLine(printError(e)); return false; }
        }
        private static async Task<bool> SendTextMessage() {
            try {
                await GameService.SendBroadcastMessageAsync(_textMessage);
                return true;
            } catch(Exception e) {
                Console.WriteLine(e.Message + "\n" + printError(e)); return false;
            }
        }

        private static void ParticipantLogin(string name) {
            var ptp = Participants.FirstOrDefault(p => string.Equals(p.Name, name));
            if(IsLoggedIn && ptp == null) {
                ctxTaskFactory.StartNew(() => Participants.Add(new User {
                    Name = name
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
            if(!string.IsNullOrEmpty(User.Name)) await GameService.LoginAsync(User.Name, User.HashedPassword);
            IsConnected = true;
            IsLoggedIn = true;
        }

        private static async void Disconnected() {
            var connectionTask = GameService.ConnectAsync();
            await connectionTask.ContinueWith(t => {
                if(!t.IsFaulted) {
                    IsConnected = true;
                    GameService.LoginAsync(User.Name, User.HashedPassword).Wait();
                    IsLoggedIn = true;
                }
            });
        }
        public static async void NewTextMessage(string author, string message) {
            string msg = author + "\n" + message;
            var sender = Participants.Where((u) => string.Equals(u.Name, author)).FirstOrDefault();

            await ctxTaskFactory.StartNew(() => Console.WriteLine(msg));
        }

        static bool ConsoleEventCallback(int eventType) {
            if(eventType == 2) {
                Logout().Wait();
            }
            return false;
        }

        static ConsoleEventDelegate handler;   // Keeps it from getting garbage collected
                                               // Pinvoke
        private delegate bool ConsoleEventDelegate(int eventType);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);
    }
}
