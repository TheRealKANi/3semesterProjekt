using PolyWars.API.Network.Services.DataContracts;
using System.Collections.Generic;
using System.ServiceModel;

namespace PolyWars.API.Network.Services {
    /// <summary>
    /// Base class for service layout
    /// </summary>
    [ServiceContract]
    public interface IWebClientService {

        [OperationContract]
        bool login(UserData userData);

        [OperationContract]
        bool register(UserData userData);

        [OperationContract]
        List<LeaderboardEntryData> GetLeaderBoard();

    }
}
