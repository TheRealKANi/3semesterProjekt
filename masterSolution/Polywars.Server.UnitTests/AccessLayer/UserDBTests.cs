using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolyWars.API.Network.Services.DataContracts;
using PolyWars.Server.AccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyWars.Server.AccessLayer.Tests {
    [TestClass()]
    public class UserDBTests {
        [TestMethod()]
        public void validLoginUserTest() {
            UserData userData = new UserData() {
                userName = "test",
                password = "123456"
            };
            Assert.IsTrue(UserDB.loginUser(userData));
        }
        [TestMethod()]
        public void invalidLoginUserTest() {
            UserData userData1 = new UserData() {
                userName = "æakfjshvgpayfsbc89+afusbåændks",
                password = "gåaoidfy7b68a7n"
            };
            Assert.IsFalse(UserDB.loginUser(userData1));
        }

        [TestMethod()]
        public void registerUserTest() {
            UserData userData1 = new UserData() {
                userName = "TestRegister",
                password = "123456"
            };
            Assert.IsTrue(UserDB.registerUser(userData1));
        }
    }
}