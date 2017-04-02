using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebServices.DAO;
using GraphServices.DTO;

namespace WebServices.Algorithms
{
    public class BfsShortestPath : IShortestPath
    {
        IGraphNodeDAO graphNodeDao;

        public BfsShortestPath(IGraphNodeDAO _graphNodeDao)
        {
            graphNodeDao = _graphNodeDao;
        }

        private string nodeIdFrom;
        private string nodeIdTo;

        NodeWrap goalNode = null;

        private Queue<NodeWrap> queue;

        private ISet<string> visitedNodes = new HashSet<string>();

        public async Task<string[]> GetShortestPath(string _nodeIdFrom, string _nodeIdTo)
        {
            if (queue != null)
            {
                throw new InvalidOperationException("instance already used");
            }

            queue = new Queue<NodeWrap>();

            nodeIdFrom = _nodeIdFrom;
            nodeIdTo = _nodeIdTo;

            await AddNode(nodeIdFrom, null);

            await RunMainLoop();

            Cleanup();

            if (goalNode == null)
            {
                //no path found
                return null;
            }

            return BacktrackPath();
        }

        private async Task RunMainLoop()
        {
            while (queue.Any())
            {
                var node = queue.Dequeue();

                foreach (var adjacentNodeId in node.Node.AdjacentNodeIDs)
                {
                    if (adjacentNodeId == nodeIdTo)
                    {
                        goalNode = await AddNode(adjacentNodeId, node);
                        return;
                    }

                    if (!visitedNodes.Contains(adjacentNodeId))
                    {
                        await AddNode(adjacentNodeId, node);
                    }
                }
            }
        }

        private async Task<NodeWrap> AddNode(string id, NodeWrap predecessor)
        {
            var node = await graphNodeDao.GetGraphNode(id);
            if (node == null)
            {
                throw new ShortestPathException($"Cannot find node ${id}");
            }

            var wrap = new NodeWrap { Node = node, Predecessor = predecessor };
            visitedNodes.Add(wrap.ID);
            queue.Enqueue(wrap);
            return wrap;
        }

        private string[] BacktrackPath()
        {
            List<string> path = new List<string>();
            NodeWrap nodeBack = goalNode;
            do
            {
                path.Add(nodeBack.ID);
                nodeBack = nodeBack.Predecessor;
            }
            while (nodeBack != null);

            return path.Reverse<string>().ToArray();
        }

        private void Cleanup()
        {
            visitedNodes.Clear();
            queue.Clear();
        }

        class NodeWrap
        {
            public string ID => Node.ID;
            public GraphNode Node { get; set; }
            public NodeWrap Predecessor { get; set; }
        }
    }
}