using PolyWars.API;
using System.Collections.Generic;

namespace PolyWars.Server {
    public class Lobby : ILobby {

        public int ID { get; private set; }
        public string Name { get; set; }
        public List<IUser> UserList { get; set; }

        public Lobby(string lobbyName, int lobbyID) {
            ID = lobbyID;
            Name = lobbyName;
            UserList = new List<IUser>();
        }
    }
}