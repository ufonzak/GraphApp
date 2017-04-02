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
    public class ShortestPathTest
    {
        private static StandardKernel kernel;

        private static Dictionary<string, GraphNode> nodeStorage;

        IShortestPath shortestPath;

        [ClassInitialize]
        public static void InitClass(TestContext ctx)
        {
            kernel = new StandardKernel();
            kernel.Bind<IShortestPath>().To<BfsShortestPath>();

            Mock<IGraphNodeDAO> graphDao = new Mock<IGraphNodeDAO>();
            graphDao.Setup(dao => dao.GetGraphNode(It.IsAny<string>())).Returns<string>(id => {
                GraphNode node;
                nodeStorage.TryGetValue(id, out node);
                return Task.FromResult(node);
            });
            kernel.Bind<IGraphNodeDAO>().ToConstant(graphDao.Object);
        }

        [TestInitialize]
        public void TestInit()
        {
            nodeStorage = null;
            shortestPath = kernel.Get<IShortestPath>();
        }

        [TestMethod]
        public async Task FindsDirectPath()
        {
            nodeStorage = new GraphNode[] {
                new GraphNode
                {
                     ID = "n1",
                     AdjacentNodeIDs = new List<string> { "n2" }
                }, new GraphNode
                {
                     ID = "n2",
                     AdjacentNodeIDs = new List<string> { "n1", "n3" }
                },new GraphNode
                {
                     ID = "n3",
                     AdjacentNodeIDs = new List<string> { "n2" }
                }
            }.ToDictionary(node => node.ID);

            var path = await shortestPath.GetShortestPath("n1", "n3");

            Assert.IsTrue(Enumerable.SequenceEqual(path, new string[] { "n1", "n2", "n3" }));
        }
    }
}
