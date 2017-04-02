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

        static bool DeepEqual(GraphNode node1, GraphNode node2)
        {
            return node1.ID == node2.ID && node1.Label == node2.Label
                && Enumerable.SequenceEqual(node1.AdjacentNodeIDs, node2.AdjacentNodeIDs);
        }
    }
}
