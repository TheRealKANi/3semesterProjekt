using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolyWars.API.Network.DTO;
using PolyWars.Server.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyWars.Server.Factories.Tests {
    [TestClass()]
    public class ResourceFactoryTests {
        [TestMethod()]
        public void generateResourcesTest() {
        int numberOfresources = 5;
            int resourcesValue = 6;
            IEnumerable<ResourceDTO> resources = ResourceFactory.generateResources(numberOfresources, resourcesValue);
            foreach(ResourceDTO resource in resources) {
                Assert.IsTrue(resource.Value == resourcesValue);
            }
            Assert.IsTrue(resources.Count() == numberOfresources);
        }
    }
}