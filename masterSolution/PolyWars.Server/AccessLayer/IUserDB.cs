using PolyWars.API.Network.Services.DataContracts;

namespace PolyWars.Server.AccessLayer {
    interface IUserDB {
        bool registerUser(UserData user);
        bool loginUser(UserData user);
    }
}
