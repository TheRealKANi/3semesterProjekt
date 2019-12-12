using PolyWars.API;
using System.Collections.Generic;
using System.Linq;

namespace PolyWars.Server {
    public class LobbyController {

        public static string DefaultLobby { get { return "mainLobby"; } }
        private static List<ILobby> lobbies;
        private static LobbyController instance;
        public static LobbyController Instance {
            get {
                if(instance == null) {
                    instance = new LobbyController();
                    lobbies.Add(new Lobby(DefaultLobby, 0));
                }
                return instance;
            }
        }
        private LobbyController() {
            lobbies = new List<ILobby>();

        }

        public static List<ILobby> listLobbies() {
            return lobbies;
        }

        public static void createLobby(string name, int id) {
            lobbies.Add(new Lobby(name, id));
        }

        public static ILobby getLobby(string name) {
            return lobbies.FirstOrDefault((l) => l.Name.Equals(name));
        }

        public static ILobby getLobby(int id) {
            return lobbies.FirstOrDefault((l) => l.ID == id);
        }

        public static List<IUser> getLobbyUsers(string name) {
            return getLobby(name).UserList;
        }

        public static List<IUser> getLobbyUsers(int id) {
            return getLobby(id).UserList;
        }


    }
}
