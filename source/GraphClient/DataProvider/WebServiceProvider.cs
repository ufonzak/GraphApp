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
            var client = Kernel.Get<GraphDataServiceClient>();
            try
            {
                return await client.GetAllGraphNodesAsync();
            }
            finally
            {
                client.Close();
            }
        }

        public async Task<string[][]> GetGraphComponents()
        {
            var client = Kernel.Get<GraphQueryServiceClient>();
            try
            {
                return await client.GetGraphComponentsAsync();
            }
            finally
            {
                client.Close();
            }
        }

        public async Task<string[]> GetShortestPath(string nodeFrom, string nodeTo)
        {
            var client = Kernel.Get<GraphQueryServiceClient>();
            try
            {
                return await client.GetShortestPathAsync(nodeFrom, nodeTo);
            }
            finally
            {
                client.Close();
            }
        }
    }
}
