using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyWars.Server {
    public interface IClient {
        void ParticipantDisconnection( string name );
        void ParticipantReconnection( string name );
        void ParticipantLogin( User client );
        void ParticipantLogout( string name );
        void BroadcastTextMessage( string sender, string message );
    }
}
