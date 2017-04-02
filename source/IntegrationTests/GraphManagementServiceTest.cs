using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IntegrationTests.Services;
using System.Collections.Generic;
using System.Linq;

namespace IntegrationTests
{
    [TestClass]
    public class GraphManagementServiceTest
    {
        [TestMethod]
        public void ServiceCreatesNode()
        {
            GraphManagementServiceClient client = new GraphManagementServiceClient();

            var graphNode = new GraphNode()
            {
                ID = "xxx",
                Label = "XXX1",
                AdjacentNodeIDs = new string[] { "yyy", "zzz" }
            };
            client.SyncGraphNode(graphNode);

            var graphNodeRetrieved = client.GetGraphNode("xxx");

            Assert.IsTrue(DeepEqual(graphNode, graphNodeRetrieved));

            client.Close();
        }

        [TestMethod]
        public void ServiceUpdatesNode()
        {
            GraphManagementServiceClient client = new GraphManagementServiceClient();

            var graphNode = new GraphNode()
            {
                ID = "xxx2",
                Label = "XXX1",
                AdjacentNodeIDs = new string[] { "yyy", "zzz" }
            };
            client.SyncGraphNode(graphNode);

            graphNode.Label = "XXX2";
            graphNode.AdjacentNodeIDs = new string[] { "ccc" };

            client.SyncGraphNode(graphNode);

            var graphNodeRetrieved = client.GetGraphNode("xxx2");

            Assert.IsTrue(DeepEqual(graphNode, graphNodeRetrieved));

            client.Close();
        }


        [TestMethod]
        public void ServiceReturnsNullForNonExistingNode()
        {
            GraphManagementServiceClient client = new GraphManagementServiceClient();

            Assert.IsNull(client.GetGraphNode("nonExisting"));

            client.Close();
        }


        [TestMethod]
        public void ServiceDeletesNodes()
        {
            GraphManagementServiceClient client = new GraphManagementServiceClient();

            var graphNode = new GraphNode()
            {
                ID = "xxx3",
                Label = "XXX1",
                AdjacentNodeIDs = new string[] { "yyy", "zzz" }
            };
            client.SyncGraphNode(graphNode);
            Assert.IsNotNull(client.GetGraphNode("xxx3"));

            client.InvalidateAllGraphNodes();
            client.DeleteAllInvalidGraphNodes();

            Assert.IsNull(client.GetGraphNode("xxx3"));

            client.Close();
        }



        [TestMethod]
        public void ServiceSynchronizesNodeCollection()
        {
            GraphManagementServiceClient client = new GraphManagementServiceClient();
            
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

            SynchronizeNodeCollection(client, nodeColection);

            Assert.IsNotNull(client.GetGraphNode("a"));
            Assert.IsNotNull(client.GetGraphNode("b"));
            Assert.IsNotNull(client.GetGraphNode("c"));

            client.Close();


            client = new GraphManagementServiceClient();

            var alteredNodeCollection = nodeColection.Where(node => node.ID != "b").ToArray();

            SynchronizeNodeCollection(client, alteredNodeCollection);

            Assert.IsNotNull(client.GetGraphNode("a"));
            Assert.IsNull(client.GetGraphNode("b"));
            Assert.IsNotNull(client.GetGraphNode("c"));

            client.Close();
        }

        private void SynchronizeNodeCollection(IGraphManagementService client, IEnumerable<GraphNode> nodes)
        {
            client.InvalidateAllGraphNodes();

            foreach (var node in nodes)
            {
                client.SyncGraphNode(node);
            }

            client.DeleteAllInvalidGraphNodes();
        }


        static bool DeepEqual(GraphNode node1, GraphNode node2)
        {
            return node1.ID == node2.ID && node1.Label == node2.Label
                && Enumerable.SequenceEqual(node1.AdjacentNodeIDs, node2.AdjacentNodeIDs);
        }
    }
}
