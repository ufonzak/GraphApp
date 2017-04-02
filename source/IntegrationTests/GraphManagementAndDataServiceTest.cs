using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using IntegrationTests.GraphManagementService;
using IntegrationTests.GraphDataService;
using GraphServices.DTO;

namespace IntegrationTests
{
    [TestClass]
    public class GraphManagementAndDataServiceTest
    {
        GraphManagementServiceClient graphManagement;
        GraphDataServiceClient graphData;

        [TestInitialize]
        public void TestInit()
        {
            graphManagement = new GraphManagementServiceClient();
            graphData = new GraphDataServiceClient();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            graphManagement.Close();
            graphData.Close();
        }

        [TestMethod]
        public void ServiceCreatesNode()
        {
            var graphNode = new GraphNode()
            {
                ID = "xxx",
                Label = "XXX1",
                AdjacentNodeIDs = new List<string> { "yyy", "zzz" }
            };
            graphManagement.SyncGraphNode(graphNode);

            var graphNodeRetrieved = graphData.GetGraphNode("xxx");

            Assert.IsTrue(DeepEqual(graphNode, graphNodeRetrieved));
        }

        [TestMethod]
        public void ServiceUpdatesNode()
        {
            var graphNode = new GraphNode()
            {
                ID = "xxx2",
                Label = "XXX1",
                AdjacentNodeIDs = new List<string> { "yyy", "zzz" }
            };
            graphManagement.SyncGraphNode(graphNode);

            graphNode.Label = "XXX2";
            graphNode.AdjacentNodeIDs = new List<string> { "ccc" };

            graphManagement.SyncGraphNode(graphNode);

            var graphNodeRetrieved = graphData.GetGraphNode("xxx2");

            Assert.IsTrue(DeepEqual(graphNode, graphNodeRetrieved));
        }


        [TestMethod]
        public void ServiceReturnsNullForNonExistingNode()
        {
            Assert.IsNull(graphData.GetGraphNode("nonExisting"));
        }

        [TestMethod]
        public void ServiceDeletesNodes()
        {
            var graphNode = new GraphNode()
            {
                ID = "xxx3",
                Label = "XXX1",
                AdjacentNodeIDs = new List<string> { "yyy", "zzz" }
            };
            graphManagement.SyncGraphNode(graphNode);
            Assert.IsNotNull(graphData.GetGraphNode("xxx3"));

            graphManagement.InvalidateAllGraphNodes();
            graphManagement.DeleteAllInvalidGraphNodes();

            Assert.IsFalse(graphData.GetAllGraphNodes().Any());
        }

        [TestMethod]
        public void ServiceSynchronizesNodeCollection()
        {
            var nodeColection = new GraphNode[] {
                new GraphNode()
                {
                     ID="a", Label="a"
                }, new GraphNode()
                {
                     ID="b", Label="b"
                }, new GraphNode()
                {
                     ID="c", Label="c"
                }
            };

            SynchronizeNodeCollection(nodeColection);

            Assert.IsTrue(Enumerable.SequenceEqual(graphData.GetAllGraphNodes().Select(node => node.ID).OrderBy(id => id), new string[] { "a", "b", "c" }));

            var alteredNodeCollection = nodeColection.Where(node => node.ID != "b").ToArray();

            SynchronizeNodeCollection(alteredNodeCollection);

            Assert.IsNull(graphData.GetGraphNode("b"));
            Assert.IsTrue(Enumerable.SequenceEqual(graphData.GetAllGraphNodes().Select(node => node.ID).OrderBy(id => id), new string[] { "a", "c" }));

        }

        private void SynchronizeNodeCollection(IEnumerable<GraphNode> nodes)
        {
            graphManagement.InvalidateAllGraphNodes();

            foreach (var node in nodes)
            {
                graphManagement.SyncGraphNode(node);
            }

            graphManagement.DeleteAllInvalidGraphNodes();
        }


        static bool DeepEqual(GraphNode node1, GraphNode node2)
        {
            return node1.ID == node2.ID && node1.Label == node2.Label
                && Enumerable.SequenceEqual(node1.AdjacentNodeIDs, node2.AdjacentNodeIDs);
        }
    }
}
