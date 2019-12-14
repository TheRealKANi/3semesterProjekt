using PolyWars.API.Network.Services.DataContracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;
using System.Threading.Tasks;

namespace PolyWars.Server.AccessLayer {
    class UserDB {

        private static IDbConnection Con = null;
        
        public static bool loginUser(UserData user) {
            bool result = false;
            string SQLLoginUser = "SELECT Vertices FROM Users WHERE Username = @userName AND HashedPassword = @password";


            // Get Salt and check agains server entry
            using(Con = new SqlConnection(DBConnection.DbConnectionString)) {
                int vertices = Con.Query<int>(SQLLoginUser, new { userName = user.userName, password = user.password }).FirstOrDefault();
                if(vertices >= 3) {
                    result = true;
                };
            }
            return result;
        }

        public static bool registerUser(UserData user) {
            string SQLRegisterUser = "insert into Users (Username, HashedPassword, Salt, Score, Vertices) values " +
                                                    "(@userName, @hashedPassword, 0, 0, 3)";
            // Generate salt for user to use on password
            string hashedPassword = user.password;
            using(Con = new SqlConnection(DBConnection.DbConnectionString)) {
                return Con.Query<bool>(SQLRegisterUser, new { userName = user.userName, hashedPassword }).FirstOrDefault();
            }
        }
    }
}
