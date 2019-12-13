using System.Runtime.Serialization;

namespace PolyWars.Server.Services {
    [DataContract]
    public class UserData {
        [DataMember]
        public string userName;

        [DataMember]
        public string password;
    }
}