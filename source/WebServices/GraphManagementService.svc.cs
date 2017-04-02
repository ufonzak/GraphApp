using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using WebServices.DAO;
using GraphServices;
using GraphServices.DTO;

namespace WebServices
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class GraphManagementService : IGraphManagementService
    {
        IGraphNodeDAO GraphNodeDAO => Kernel.Get<IGraphNodeDAO>();

        public async Task DeleteAllInvalidGraphNodes()
        {
            await GraphNodeDAO.DeleteAllInvalidGraphNodes();
        }

        public async Task InvalidateAllGraphNodes()
        {
            await GraphNodeDAO.InvalidateAllGraphNodes();
        }

        public async Task SyncGraphNode(GraphNode node)
        {
            await GraphNodeDAO.SyncGraphNode(node);
        }
    }
}
