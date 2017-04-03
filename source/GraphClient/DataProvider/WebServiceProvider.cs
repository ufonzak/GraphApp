using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphServices.DTO;
using GraphClient.GraphDataService;
using GraphClient.GraphQueryService;

namespace GraphClient.DataProvider
{
    class WebServiceProvider : IGraphDataProvider
    {
        public async Task<GraphNode[]> GetAllNodes()
        {
            using (var client = Kernel.Get<GraphDataServiceClient>())
            {
                return await client.GetAllGraphNodesAsync();
            }
        }

        public async Task<string[][]> GetGraphComponents()
        {
            using (var client = Kernel.Get<GraphQueryServiceClient>())
            {
                return await client.GetGraphComponentsAsync();
            }
        }

        public async Task<string[]> GetShortestPath(string nodeFrom, string nodeTo)
        {
            using (var client = Kernel.Get<GraphQueryServiceClient>())
            {
                return await client.GetShortestPathAsync(nodeFrom, nodeTo);
            }
        }
    }
}
