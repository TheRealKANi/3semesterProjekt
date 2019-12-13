using PolyWars.API.Network.Services.DataContracts;
using PolyWars.Server.WebService.ConsoleClient.PolyWarsWebClientService;
using System;

namespace PolyWars.Server.WebService.ConsoleClient {
    class Program {
        static void Main(string[] args) {
            WebClientServiceClient client = new WebClientServiceClient();

            UserData user = new UserData() {
                userName = "Silas",
                password = "ThureStucks"
            };
            Console.WriteLine($"Trying to register '{user.userName}' with pass: '{user.password}'");
            bool registerResult = client.register(user);
            Console.WriteLine($"Register result: {registerResult}\n");

            Console.WriteLine($"Trying to login with user: '{user.userName}' and pass: '{user.password}'");
            bool loginResult = client.login(user);
            Console.WriteLine($"Login result: {loginResult}\n");


            Console.WriteLine("Trying to get LeaderBoard to Console:\n");
            LeaderboardEntryData[] leaderBoard = client.GetLeaderBoard();
            for(int i = 0; i < leaderBoard.Length; i++) {
                Console.WriteLine($"{i + 1} Place: {leaderBoard[i].userName} - {leaderBoard[i].score} Points");
            }
            Console.WriteLine("");
            client.Close();
            Console.ReadLine();
        }
    }
}
