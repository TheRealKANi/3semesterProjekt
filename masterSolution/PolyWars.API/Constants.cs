namespace PolyWars.API {

    /// <summary>
    /// Constants that enables client and server to have uniform data
    /// </summary>
    public static class Constants {
        public static readonly string[] serverList = new string[] {
            "localhost",
            "PolyWars.LeetFix.dk",
            "PolyWars.ServeGame.com"
        };
        public static readonly string protocol = "http://";
        public static readonly string serverPort = "5700";
        public static readonly string serverEndPoint = "/GameService";
        public static readonly int standardShotDamage = 20;

        public static readonly string servicePort = "5701";
        public static readonly string serviceEndPoint = "/WebClientService";

    }
}
