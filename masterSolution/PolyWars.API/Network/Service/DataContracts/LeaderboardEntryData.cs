using System.Runtime.Serialization;

namespace PolyWars.API.Network.Services.DataContracts {
    [DataContract]
    public class LeaderboardEntryData {

        [DataMember]
        public string userName;

        [DataMember]
        public string score;
    }
}