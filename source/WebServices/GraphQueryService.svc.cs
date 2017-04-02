using GraphServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WebServices.Algorithms;

namespace WebServices
{
    public class GraphQueryService : IGraphQueryService
    {
        public async Task<string[][]> GetGraphComponents()
        {
            IGraphComponents components = Kernel.Get<IGraphComponents>();
            return await components.GetGraphComponents();
        }

        public async Task<string[]> GetShortestPath(string nodeIdFrom, string nodeIdTo)
        {
            IShortestPath shortestPath = Kernel.Get<IShortestPath>();
            return await shortestPath.GetShortestPath(nodeIdFrom, nodeIdTo);
        }
    }
}
