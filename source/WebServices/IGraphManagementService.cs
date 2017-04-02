using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WebServices.DAO;
using System.Threading.Tasks;

namespace WebServices
{
    [ServiceContract]
    public interface IGraphManagementService
    {
        [OperationContract]
        Task<GraphNode> GetGraphNode(string id);

        [OperationContract]
        Task SyncGraphNode(GraphNode node);

        [OperationContract]
        Task InvalidateAllGraphNodes();

        [OperationContract]
        Task DeleteAllInvalidGraphNodes();
    }
}
