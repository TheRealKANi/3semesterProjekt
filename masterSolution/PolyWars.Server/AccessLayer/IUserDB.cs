using PolyWars.API.Network.Services.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyWars.Server.AccessLayer {
    interface IUserDB {
        bool registerUser(UserData user);
        bool loginUser(UserData user);
    }
}
