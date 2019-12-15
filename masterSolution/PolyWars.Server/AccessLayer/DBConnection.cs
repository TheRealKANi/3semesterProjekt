using System.Configuration;
using System.Data;

namespace PolyWars.Server.AccessLayer {

    class DBConnection {
        public static string DbConnectionString {
            get { return ConfigurationManager.ConnectionStrings["AzureSQLConnectionString"].ToString(); }
        }
    }
}
