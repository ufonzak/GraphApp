using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphServices.DTO;

namespace WebServices.DAO
{
    public interface IGraphNodeDAO
    {
        Task SyncGraphNode(GraphNode node);

        Task<GraphNode> GetGraphNode(string id);

        Task<GraphNode[]> GetAllGraphNodes();

        Task InvalidateAllGraphNodes();

        Task DeleteAllInvalidGraphNodes();
    }
}
