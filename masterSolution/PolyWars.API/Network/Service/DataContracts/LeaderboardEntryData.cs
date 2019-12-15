using System.Runtime.Serialization;

namespace PolyWars.API.Network.Services.DataContracts {
    /// <summary>
    /// Base class for transfering entries from leaderboard on server
    /// thru service to webclients
    /// </summary>
    [DataContract]
    public class LeaderboardEntryData {

        [DataMember]
        public string userName;

        [DataMember]
        public string score;
    }
}