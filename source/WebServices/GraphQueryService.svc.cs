using GraphServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WebServices
{
    public class GraphQueryService : IGraphQueryService
    {
        public async Task<string[]> GetShortestPath(string nodeIdFrom, string nodeIdTo)
        {
            throw new NotImplementedException();
        }
    }
}
