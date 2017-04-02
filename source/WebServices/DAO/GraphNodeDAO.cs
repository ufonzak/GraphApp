using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Diagnostics;
using System.Threading.Tasks;

namespace WebServices.DAO
{
    public class GraphNodeDAO : IGraphNodeDAO
    {
        IMongoCollection<GraphNode> Nodes => Kernel.Get<IMongoDatabase>().GetCollection<GraphNode>("graphNodes");

        public GraphNodeDAO()
        {

        }

        public async Task<GraphNode> GetGraphNode(string id)
        {
            return await Nodes.Find(Builders<GraphNode>.Filter.Eq("_id", id)).FirstOrDefaultAsync();
        }

        public async Task SyncGraphNode(GraphNode node)
        {
            node.IsValid = true;
            await Nodes.ReplaceOneAsync(Builders<GraphNode>.Filter.Eq("_id", node.ID), node, new UpdateOptions() { IsUpsert = true });
        }

        public async Task InvalidateAllGraphNodes()
        {
            await Nodes.UpdateManyAsync(Builders<GraphNode>.Filter.Empty, Builders<GraphNode>.Update.Set(n => n.IsValid, false));
        }

        public async Task DeleteAllInvalidGraphNodes()
        {
            await Nodes.DeleteManyAsync(Builders<GraphNode>.Filter.Eq(n => n.IsValid, false));
        }
    }
}