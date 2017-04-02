using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;
using System.Diagnostics;
using System.Threading.Tasks;

namespace WebServices.DAO
{
    public class GraphNodeDAO : IGraphNodeDAO
    {
        public GraphNodeDAO()
        {
        }

        public async Task<GraphNode> GetGraphNode(string id)
        {
            return storage[id];
        }

        public async Task SyncGraphNode(GraphNode node)
        {
            storage[node.ID] = node;
        }

        private Dictionary<string, GraphNode> storage = new Dictionary<string, GraphNode>();
    }
}