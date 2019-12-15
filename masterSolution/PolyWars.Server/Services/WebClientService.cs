using PolyWars.API.Network.DTO;
using PolyWars.API.Network.Services.DataContracts;
using PolyWars.Server.AccessLayer;
using System.Collections.Generic;
using System.Diagnostics;

namespace PolyWars.Server.Services {
    public class WebClientService : IWebClientService {

        /// <summary>
        /// Returns a list of currently alive opponents to caller
        /// </summary>
        /// <returns></returns>
        public List<LeaderboardEntryData> GetLeaderBoard() {
            List<LeaderboardEntryData> leaderBoard = new List<LeaderboardEntryData>();
            foreach(PlayerDTO currentPlayer in MainHub.getLeaderBoard()) {
                leaderBoard.Add(new LeaderboardEntryData() { score = currentPlayer.Wallet.ToString(), userName = currentPlayer.Name });
            }
            return leaderBoard;
        }

        /// <summary>
        /// attempts to login user with supplied creds
        /// </summary>
        /// <param name="userData">The user to login</param>
        /// <returns></returns>
        public bool login(UserData userData) {
            bool result = UserDB.loginUser(userData);
            Debug.WriteLine($"User: '{userData.userName}' tried to login with password: '{userData.password}' - Returns {result}");
            return result;
        }

        /// <summary>
        /// attempts to register user with supplied creds
        /// </summary>
        /// <param name="userData">The user to register</param>
        /// <returns></returns>
        public bool register(UserData userData) {
            bool result = UserDB.registerUser(userData);
            Debug.WriteLine($"User: '{userData.userName}' tried to register with password: '{userData.password}' - Returns {result}");
            return result;
        }
    }
}
