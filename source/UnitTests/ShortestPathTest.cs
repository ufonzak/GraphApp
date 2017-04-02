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
            graphDao.Setup(dao => dao.GetGraphNode(It.IsAny<string>())).Returns<string>(id =>
            {
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


        [TestMethod]
        public async Task ReturnsNullForNonExistingPath()
        {
            nodeStorage = new GraphNode[] {
                new GraphNode
                {
                     ID = "n1",
                     AdjacentNodeIDs = new List<string> { "n1" }
                }, new GraphNode
                {
                     ID = "n2",
                     AdjacentNodeIDs = new List<string> { "n2" }
                }
            }.ToDictionary(node => node.ID);

            var path = await shortestPath.GetShortestPath("n1", "n2");

            Assert.IsNull(path);
        }


        [TestMethod]
        public async Task ReturnsPathForSelfLoop()
        {
            nodeStorage = new GraphNode[] {
                new GraphNode
                {
                     ID = "n1",
                     AdjacentNodeIDs = new List<string> { "n1" }
                }
            }.ToDictionary(node => node.ID);

            var path = await shortestPath.GetShortestPath("n1", "n1");

            Assert.IsTrue(Enumerable.SequenceEqual(path, new string[] { "n1", "n1" }));
        }


        [TestMethod]
        [ExpectedException(typeof(ShortestPathException))]
        public async Task ThrowsExceptionForNonExistingNodes()
        {
            nodeStorage = new GraphNode[] {
                new GraphNode
                {
                     ID = "n1",
                     AdjacentNodeIDs = new List<string> { "n1", "n2" }
                }
            }.ToDictionary(node => node.ID);

            await shortestPath.GetShortestPath("n1", "n2");
        }

        [TestMethod]
        public async Task FindsPathInComplexScenario()
        {
            nodeStorage = new GraphNode[] {
                new GraphNode
                {
                     ID = "n1",
                     AdjacentNodeIDs = new List<string> { "n4", "n1" }
                }, new GraphNode
                {
                     ID = "n2",
                     AdjacentNodeIDs = new List<string> { "n4", "n3", "n5" }
                },new GraphNode
                {
                     ID = "n3",
                     AdjacentNodeIDs = new List<string> { "n2", "n5", "n3" }
                },new GraphNode
                {
                     ID = "n4",
                     AdjacentNodeIDs = new List<string> { "n2", "n5", "n6", "n1" }
                },new GraphNode
                {
                     ID = "n5",
                     AdjacentNodeIDs = new List<string> { "n2", "n3", "n4", "n6", "n7" }
                },new GraphNode
                {
                     ID = "n6",
                     AdjacentNodeIDs = new List<string> { "n4", "n5" }
                },new GraphNode
                {
                     ID = "n7",
                     AdjacentNodeIDs = new List<string> { "n7", "n5" }
                }
            }.ToDictionary(node => node.ID);

            var path = await shortestPath.GetShortestPath("n1", "n7");

            Assert.IsTrue(Enumerable.SequenceEqual(path, new string[] { "n1", "n4", "n5", "n7" }));
        }
    }
}
