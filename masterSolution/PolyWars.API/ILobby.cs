using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyWars.API {
    public interface ILobby {
        int ID { get; }
        string Name { get; set; }
        List<IUser> UserList { get; set; }
    }
}
