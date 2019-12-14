using PolyWars.API.Network.DTO;
using PolyWars.API.Network.Services.DataContracts;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PolyWars.Server.Services {
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "WebClientService" in both code and config file together.
    public class WebClientService : IWebClientService {
        public List<LeaderboardEntryData> GetLeaderBoard() {
            List<LeaderboardEntryData> leaderBoard = new List<LeaderboardEntryData>();
            foreach(PlayerDTO currentPlayer in MainHub.getLeaderBoard()) {
                leaderBoard.Add(new LeaderboardEntryData() { score = currentPlayer.Wallet.ToString(), userName = currentPlayer.Name });
            }
            return leaderBoard;
        }

        public bool login(UserData userData) {
            Debug.WriteLine($"User: '{userData.userName}' tried to login with password: '{userData.password}' - Returns True");
            return true;
        }

        public bool register(UserData userData) {
            Debug.WriteLine($"User: '{userData.userName}' tried to register with password: '{userData.password}' - Returns True");
            return true;
        }
    }
}
