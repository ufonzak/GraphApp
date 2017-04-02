using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ninject;
using WebServices.DAO;
using WebServices.Algorithms;
using GraphServices.DTO;
using System.Threading.Tasks;
using System.Linq;

namespace UnitTests
{
    [TestClass]
    public class GraphComponentsTest
    {
        private static StandardKernel kernel;

        private static GraphNode[] nodeStorage;

        IGraphComponents graphComponents;

        [ClassInitialize]
        public static void InitClass(TestContext ctx)
        {
            kernel = new StandardKernel();
            kernel.Bind<IGraphComponents>().To<DfsGraphComponents>();

            Mock<IGraphNodeDAO> graphDao = new Mock<IGraphNodeDAO>();
            graphDao.Setup(dao => dao.GetAllGraphNodes()).Returns(() => Task.FromResult(nodeStorage));
            kernel.Bind<IGraphNodeDAO>().ToConstant(graphDao.Object);
        }

        [TestInitialize]
        public void TestInit()
        {
            nodeStorage = null;
            graphComponents = kernel.Get<IGraphComponents>();
        }
        
        [TestMethod]
        public async Task ResolvesComponents()
        {
            nodeStorage = new GraphNode[] {
                new GraphNode
                {
                     ID = "n1",
                     AdjacentNodeIDs = new List<string> { "n1", "n2" }
                }, new GraphNode
                {
                     ID = "n2",
                     AdjacentNodeIDs = new List<string> { "n1", "n3" }
                }, new GraphNode
                {
                     ID = "n3",
                     AdjacentNodeIDs = new List<string> { "n1" }
                }, new GraphNode
                {
                     ID = "n4",
                     AdjacentNodeIDs = new List<string> { "n5", "n4" }
                }, new GraphNode
                {
                     ID = "n5",
                     AdjacentNodeIDs = new List<string> { "n4", "n5" }
                }, new GraphNode
                {
                     ID = "n6"
                }
            };

            var components = await graphComponents.GetGraphComponents();
            Assert.AreEqual(3, components.Length);

            var sorted = components.OrderBy(component => components.Length).ToArray();
            Assert.IsTrue(Enumerable.SequenceEqual(sorted[0].OrderBy(id => id), new string[] { "n1", "n2", "n3" }));
            Assert.IsTrue(Enumerable.SequenceEqual(sorted[1].OrderBy(id => id), new string[] { "n4", "n5" }));
            Assert.IsTrue(Enumerable.SequenceEqual(sorted[2].OrderBy(id => id), new string[] { "n6" }));
        }
    }
}
