using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using GraphServices.DTO;

namespace GraphServices
{
    [ServiceContract]
    public interface IGraphManagementService
    {
        [OperationContract]
        Task SyncGraphNode(GraphNode node);

        [OperationContract]
        Task InvalidateAllGraphNodes();

        [OperationContract]
        Task DeleteAllInvalidGraphNodes();
    }
}
