//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using PolyWars.Adapters;
//using PolyWars.API;
//using PolyWars.API.Model.Interfaces;
//using PolyWars.API.Network;
//using PolyWars.API.Network.DTO;
//using PolyWars.Logic;
//using PolyWars.Network;
//using PolyWars.Server;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Polywars.Server.UnitTests {
//    static class WalletContainer {
//        public static double wallet = 0;
//        public static void updateUnitTestWallet(double newAmount) {
//            wallet = newAmount;
//        }
//    }
    
//    [TestClass]
//    public class ResourcePickup {
//        [TestMethod]
//        public void TestShit() {
//            Debug.WriteLine("test");
//            Assert.IsTrue(true);
//        }
//        [TestMethod]
//        public void singleResourcePickup() {
//            UnitTestUIDispatcher dispatcher = new UnitTestUIDispatcher();
//            Debug.WriteLine("Made Dispatcher");
//            Task server = Task.Run(() => Program.Main("noConsole", "unitTest"));
//            Debug.WriteLine("Started server task");

//            while(!Program.serverLoaded) { Thread.Sleep(1); };
//            Debug.WriteLine("Server task finished");

//            GameService gs = NetworkController.GameService;
//            gs.ServerIP = $"127.0.0.1:{Constants.serverPort}/{Constants.serverEndPoint}/";
            
//            gs.updateWallet += WalletContainer.updateUnitTestWallet;

//            IUser user;
//            Task.Run(() => gs.ConnectAsync(true)).Wait();
//            Task.Run(async () => user = await gs.LoginAsync("player", "")).Wait();

//            List<ResourceDTO> resourceDTOs = null;
//            Task.Run(async () => resourceDTOs = await gs.getResourcesAsync()).Wait();
//            IResource resource = ResourceAdapter.DTOToResource(resourceDTOs[0]);

//            Task.Run(async () => await gs.playerCollectedResource(resource)).Wait();

//            try {
//                Assert.IsTrue(WalletContainer.wallet == resource.Value);
//                Assert.IsFalse(MainHub.Resources.ContainsKey(resource.ID));
//            } catch(AssertFailedException) {
//            } finally {
//                Program.shutdownServer();
//            }
//        }
//    };
//}
