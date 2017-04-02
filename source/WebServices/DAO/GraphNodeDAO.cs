using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;
using GraphServices.DTO;
using System.Threading.Tasks;

namespace WebServices.DAO
{
    public class GraphNodeDAO : IGraphNodeDAO
    {
        IMongoCollection<GraphNodeModel> Nodes => Kernel.Get<IMongoDatabase>().GetCollection<GraphNodeModel>("graphNodes");

        public GraphNodeDAO()
        {

        }

        public async Task<GraphNode> GetGraphNode(string id)
        {
            GraphNodeModel model = await Nodes.Find(Builders<GraphNodeModel>.Filter.Eq("_id", id)).FirstOrDefaultAsync();
            if (model == null)
            {
                return null;
            }

            return model.ToDTO();
        }

        public async Task<GraphNode[]> GetAllGraphNodes()
        {
            var nodes = await Nodes.Find(Builders<GraphNodeModel>.Filter.Empty).ToListAsync();
            return nodes.Select(node => node.ToDTO()).ToArray();
        }

        public async Task SyncGraphNode(GraphNode node)
        {
            await Nodes.UpdateOneAsync(Builders<GraphNodeModel>.Filter.Eq("_id", node.ID),
                Builders<GraphNodeModel>.Update
                    .Set(n => n.Label, node.Label)
                    .Set(n => n.AdjacentNodeIDs, node.AdjacentNodeIDs)
                    .Set(n => n.IsValid, true),
                new UpdateOptions() { IsUpsert = true });
        }

        public async Task InvalidateAllGraphNodes()
        {
            await Nodes.UpdateManyAsync(Builders<GraphNodeModel>.Filter.Empty, Builders<GraphNodeModel>.Update.Set(n => n.IsValid, false));
        }

        public async Task DeleteAllInvalidGraphNodes()
        {
            await Nodes.DeleteManyAsync(Builders<GraphNodeModel>.Filter.Eq(n => n.IsValid, false));
        }

        public async Task NormalizeRelations()
        {
            var cursor = await Nodes.Find(Builders<GraphNodeModel>.Filter.Empty).ToCursorAsync();
            await cursor.ForEachAsync(async node =>
            {
                if (node.AdjacentNodeIDs == null || node.AdjacentNodeIDs.Count == 0)
                {
                    return;
                }
                await Nodes.UpdateManyAsync(
                    Builders<GraphNodeModel>.Filter.In("_id", node.AdjacentNodeIDs),
                    Builders<GraphNodeModel>.Update.AddToSet(n => n.AdjacentNodeIDs, node.ID));
            });
        }
    }
}