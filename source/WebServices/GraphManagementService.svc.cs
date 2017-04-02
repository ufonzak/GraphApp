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
    public class GraphManagementService : IGraphManagementService
    {
        public async Task<GraphNode> GetGraphNode(string id)
        {
            throw new NotImplementedException();
        }

        public async Task SyncGraphNode(GraphNode node)
        {
            throw new NotImplementedException();
        }
    }
}
