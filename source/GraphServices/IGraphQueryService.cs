using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GraphServices
{
    [ServiceContract]
    public interface IGraphQueryService
    {
        [OperationContract]
        Task<string[]> GetShortestPath(string nodeIdFrom, string nodeIdTo);

        [OperationContract]
        Task<string[][]> GetGraphComponents();
    }
}
