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
    public interface IGraphDataService
    {
        [OperationContract]
        Task<GraphNode> GetGraphNode(string id);

        [OperationContract]
        Task<GraphNode[]> GetAllGraphNodes();
    }
}
