using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphServices.DTO;

namespace GraphClient.DataProvider
{
    class TestGraphDataProvider : IGraphDataProvider
    {
        public const int NODE_COUNT = 10; 
        public const int EDGE_COUNT = 25; 

        public async Task<GraphNode[]> GetAllNodes()
        {
            await Task.Delay(1000);

            Random rand = new Random();

            var nodes = Enumerable.Range(1, NODE_COUNT).Select(id => new GraphNode
            {
                ID = id.ToString(),
                Label = $"N{id}",
                AdjacentNodeIDs = new List<string>()
            }).ToArray();

            for (int i = 0; i < EDGE_COUNT; i++)
            {
                var node1 = nodes[rand.Next(nodes.Length)];
                var node2 = nodes[rand.Next(nodes.Length)];

                if (node1.AdjacentNodeIDs.Contains(node2.ID))
                {
                    i--;
                    continue;
                }

                node1.AdjacentNodeIDs.Add(node2.ID);
                node2.AdjacentNodeIDs.Add(node1.ID);
            }

            return nodes;
        }

        public async Task<string[]> GetShortestPath(string nodeFrom, string nodeTo)
        {
            return new string[] { nodeFrom, nodeTo };
        }
    }
}
