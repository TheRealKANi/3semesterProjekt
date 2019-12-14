using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolyWars.API;
using PolyWars.API.Model.Interfaces;
using PolyWars.API.Network;
using PolyWars.API.Network.DTO;
using PolyWars.Logic;
using PolyWars.Network;
using PolyWars.Server;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Polywars.Server.UnitTests.Resources {
    [TestClass]
    public class NonConcurrentResourcePickup {
        double wallet = 0;
        private void updateWallet(double amount) {
            wallet = amount;
        }
        [TestMethod]
        public void singlePickup() {
            //Debug.WriteLine($"{i++}");
            UnitTestUIDispatcher dispatcher = new UnitTestUIDispatcher();
            Task server = Task.Run(() => Program.Main("noConsole", "unitTest"));

            while(!PolyWars.Server.Program.serverLoaded) { Thread.Sleep(1); };

            GameService gs = new GameService() {
                ServerIP = $"127.0.0.1:{Constants.serverPort}/{Constants.serverEndPoint}/"
            };
            gs.updateWallet += updateWallet;

            bool connected = false;
            Task.Run(async () => { connected = await gs.ConnectAsync(true); }).Wait();
            Assert.IsTrue(connected, "Connection to server could not be established");

            IUser user = null;
            Task.Run(async () => { user = await gs.LoginAsync("player1", ""); }).Wait();

            List<ResourceDTO> resources = null;
            Task.Run(async () => { resources = await gs.getResourcesAsync(); }).Wait();
            Assert.IsTrue(resources != null);
            Assert.IsTrue(resources.Count > 0, "No resources in collection");

            IResource resource = PolyWars.Adapters.ResourceAdapter.DTOToResource(resources[0]);
            bool test = false;
            Task.Run(async () => test = await gs.playerCollectedResource(resource)).Wait();

            Thread.Sleep(10000); //sleeps a bit longer than the server update time

            Assert.IsTrue(wallet == resource.Value);
            Assert.IsTrue(!MainHub.Resources.ContainsKey(resource.ID));
        }
    }
}
