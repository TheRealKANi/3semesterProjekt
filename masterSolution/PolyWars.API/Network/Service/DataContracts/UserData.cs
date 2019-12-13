using System.Runtime.Serialization;

namespace PolyWars.API.Network.Services.DataContracts {
    [DataContract]
    public class UserData {
        [DataMember]
        public string userName;

        [DataMember]
        public string password;
    }
}