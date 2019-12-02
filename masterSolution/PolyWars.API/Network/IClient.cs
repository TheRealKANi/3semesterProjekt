namespace PolyWars.API.Network {
    public interface IClient {
        void ClientDisconnected(string name);
        void ClientReconnected(string name);
        void ClientLogin(IUser client);
        void ClientLogout(string name);
        void BroadcastTextMessage(string sender, string message);
    }
}
