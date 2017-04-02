using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using WebServices.DAO;

namespace WebServices
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class GraphManagementService : IGraphManagementService
    {
        public async Task DeleteAllInvalidGraphNodes()
        {
            var graphNodeDAO = Kernel.Get<IGraphNodeDAO>();

            await graphNodeDAO.DeleteAllInvalidGraphNodes();
        }

        public async Task<GraphNode> GetGraphNode(string id)
        {
            var graphNodeDAO = Kernel.Get<IGraphNodeDAO>();

            return await graphNodeDAO.GetGraphNode(id);
        }

        public async Task InvalidateAllGraphNodes()
        {
            var graphNodeDAO = Kernel.Get<IGraphNodeDAO>();

            await graphNodeDAO.InvalidateAllGraphNodes();
        }

        public async Task SyncGraphNode(GraphNode node)
        {
            var graphNodeDAO = Kernel.Get<IGraphNodeDAO>();

            await graphNodeDAO.SyncGraphNode(node);
        }
    }
}
