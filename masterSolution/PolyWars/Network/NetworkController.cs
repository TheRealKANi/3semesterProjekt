namespace PolyWars.Network {
    static class NetworkController {
        static NetworkController() {
            GameService = new GameService();
        }
        public static GameService GameService { get; private set; }
    }
}
