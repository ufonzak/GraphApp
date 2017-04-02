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
            GraphNode node;
            if (!storage.TryGetValue(id, out node))
            {
                return null;
            }

            return node;
        }

        public async Task SyncGraphNode(GraphNode node)
        {
            node.IsValid = true;
            storage[node.ID] = node;
        }

        public async Task InvalidateAllGraphNodes()
        {
            foreach (var node in storage.Values)
            {
                node.IsValid = false;
            }
        }

        public async Task DeleteAllInvalidGraphNodes()
        {
            foreach (var key in storage
                .Where(pair => !pair.Value.IsValid)
                .Select(pair => pair.Key)
                .ToArray())
            {
                storage.Remove(key);
            }
        }

        private Dictionary<string, GraphNode> storage = new Dictionary<string, GraphNode>();
    }
}