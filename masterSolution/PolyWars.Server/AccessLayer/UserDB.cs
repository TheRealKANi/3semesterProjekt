using Dapper;
using PolyWars.API.Network.Services.DataContracts;
using PolyWars.Server.Utility.Hashing;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;

namespace PolyWars.Server.AccessLayer {
    class UserDB {

        private static IDbConnection Con = null;

        public static bool loginUser(UserData user) {
            bool result = false;
            string SQLLoginUser = "SELECT Vertices FROM Users WHERE Username = @userName AND HashedPassword = @password";

            PasswordHashing ph = new PasswordHashing();
            // Get Salt and check agains server entry
            string salt = getUserSalt(user);
            if(salt != null) {
                HashWithSaltResult hashResult = ph.HashWithSalt(user.password, salt, SHA512.Create());
                using(Con = new SqlConnection(DBConnection.DbConnectionString)) {
                    int vertices = Con.Query<int>(SQLLoginUser, new { userName = user.userName, password = hashResult.CipherText }).FirstOrDefault();
                    if(vertices >= 3) {
                        result = true;
                    };
                }
            };
            return result;
        }

        private static string getUserSalt(UserData user) {
            string SQLGetUserSalt = "Select salt from Users where Username = @userName";
            using(Con = new SqlConnection(DBConnection.DbConnectionString)) {
                return Con.Query<string>(SQLGetUserSalt, new { userName = user.userName }).FirstOrDefault();
            }
        }

        public static bool registerUser(UserData user) {
            bool result = false;
            string SQLRegisterUser = "insert into Users (Username, HashedPassword, Salt, Score, Vertices) values " +
                                                    "(@userName, @cipherText, @salt, 0, 3)";
            // Generate salt for user to use on password
            PasswordHashing ph = new PasswordHashing();
            HashWithSaltResult hashResult = ph.HashWithSalt(user.password, 64, SHA512.Create());

            using(Con = new SqlConnection(DBConnection.DbConnectionString)) {
                int insertedAfflicted = Con.Execute(SQLRegisterUser, new { userName = user.userName, cipherText = hashResult.CipherText, salt = hashResult.Salt });
                if(insertedAfflicted > 0) {
                    result = true;
                }
            }
            return result;
        }
    }
}
