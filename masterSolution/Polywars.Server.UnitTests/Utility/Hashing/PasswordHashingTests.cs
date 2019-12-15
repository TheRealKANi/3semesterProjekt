using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography;

namespace PolyWars.Server.Utility.Hashing.Tests {
    [TestClass()]
    public class PasswordHashingTests {
        [TestMethod()]
        public void HashWithSaltTest() {
            PasswordHashing ph = new PasswordHashing();
            string password = "Password!";
            HashWithSaltResult hashResult = ph.HashWithSalt(password, 64, SHA512.Create());
            HashWithSaltResult hashResult2 = ph.HashWithSalt(password, hashResult.Salt, SHA512.Create());
            Assert.AreEqual<string>(hashResult.CipherText, hashResult2.CipherText);
        }
    }
}