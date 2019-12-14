using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolyWars.Model;

namespace PolyWars.Client.UnitTests {
    [TestClass]
    public class GameControllerTests {
        [TestMethod]
        public void testDeltaTime() {
            Ticker t = new Ticker();

            double dt5 = t.DeltaTime(1000d / 5);
            double dt10 = t.DeltaTime(1000d / 10);
            double dt15 = t.DeltaTime(1000d / 15);
            double dt30 = t.DeltaTime(1000d / 30);
            double dt60 = t.DeltaTime(1000d / 60);
            double dt120 = t.DeltaTime(1000d / 120);
            double dt240 = t.DeltaTime(1000d / 240);
            double dt480 = t.DeltaTime(1000d / 480);

            Assert.AreEqual(12, dt5);
            Assert.AreEqual(6, dt10);
            Assert.AreEqual(4, dt15);
            Assert.AreEqual(2, dt30);
            Assert.AreEqual(1d, dt60);
            Assert.AreEqual(0.5, dt120);
            Assert.AreEqual(0.25, dt240);
            Assert.AreEqual(0.125, dt480);
        }
    }
}
