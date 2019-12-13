using System.Collections.Generic;
using System.Diagnostics;

namespace PolyWars.Server.Services {
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "WebClientService" in both code and config file together.
    public class WebClientService : IWebClientService {
        public List<LeaderboardEntryData> GetLeaderBoard() {
            List<LeaderboardEntryData> lb = new List<LeaderboardEntryData>();
            lb.Add(new LeaderboardEntryData() { score = "1337", userName = "Kani" });
            lb.Add(new LeaderboardEntryData() { score = "1", userName = "Thure" });
            lb.Add(new LeaderboardEntryData() { score = "To Emberrasing to show", userName = "Silas" });
            return lb;
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
