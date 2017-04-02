using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServices.DAO
{
    interface IGraphNodeDAO
    {
        Task SyncGraphNode(GraphNode node);

        Task<GraphNode> GetGraphNode(string id);

        Task InvalidateAllGraphNodes();

        Task DeleteAllInvalidGraphNodes();
    }
}
