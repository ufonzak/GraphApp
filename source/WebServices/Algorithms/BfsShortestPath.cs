using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebServices.DAO;

namespace WebServices.Algorithms
{
    public class BfsShortestPath : IShortestPath
    {
        GraphNodeDAO _graphNodeDao;

        public BfsShortestPath(IGraphNodeDAO graphNodeDao)
        {
            graphNodeDao = _graphNodeDao;
        }

        private string nodeIdFrom;
        private string nodeIdTo;

        public async Task<string[]> GetShortestPath(string _nodeIdFrom, string _nodeIdTo)
        {
            nodeIdFrom = _nodeIdFrom;
            nodeIdTo = _nodeIdTo;

            return new string[] { "n1", "n2", "n3" };
        }
    }
}