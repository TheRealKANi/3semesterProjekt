using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyWars.Server {
    interface IClient {
        void ParticipantDisconnection( string name );
        void ParticipantReconnection( string name, string password );
        void ParticipantLogin( User client );
        void ParticipantLogout( string name );
    }
}
