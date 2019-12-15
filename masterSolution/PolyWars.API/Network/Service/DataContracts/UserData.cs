using System.Runtime.Serialization;

namespace PolyWars.API.Network.Services.DataContracts {
    /// <summary>
    /// Base class for transfering a user from and to the server
    /// via service
    /// </summary>
    [DataContract]
    public class UserData {
        [DataMember]
        public string userName;

        [DataMember]
        public string password;
    }
}