using System.Runtime.Serialization;

namespace PolyWars.Server.Services {
    [DataContract]
    public class LeaderboardEntryData {

        [DataMember]
        public string userName;

        [DataMember]
        public string score;
    }
}