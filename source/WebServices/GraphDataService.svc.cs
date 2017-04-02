using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WebServices.DAO;
using GraphServices;
using GraphServices.DTO;

namespace WebServices
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class GraphDataService : IGraphDataService
    {
        IGraphNodeDAO GraphNodeDAO => Kernel.Get<IGraphNodeDAO>();

        public async Task<GraphNode[]> GetAllGraphNodes()
        {
            return await GraphNodeDAO.GetAllGraphNodes();
        }

        public async Task<GraphNode> GetGraphNode(string id)
        {
            return await GraphNodeDAO.GetGraphNode(id);
        }
    }
}
