using GraphServices.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebServices.DAO;

namespace WebServices.Algorithms
{
    public class DfsGraphComponents : IGraphComponents
    {
        IGraphNodeDAO graphNodeDao;

        Dictionary<string, NodeWrap> nodesDictionary;
        Queue<NodeWrap> workQueue;

        List<NodeWrap> stack = new List<NodeWrap>();

        public DfsGraphComponents(IGraphNodeDAO _graphNodeDao)
        {
            graphNodeDao = _graphNodeDao;
        }

        public async Task<string[][]> GetGraphComponents()
        {
            var nodes = await graphNodeDao.GetAllGraphNodes();
            nodesDictionary = nodes
                .Select(node => new NodeWrap { Node = node })
                .ToDictionary(node => node.ID);

            workQueue = new Queue<NodeWrap>(nodesDictionary.Values);

            while (workQueue.Any())
            {
                var initial = workQueue.Dequeue();
                if (initial.Component != null)
                {
                    continue;
                }

                RunDFS(initial);
            }

            return nodesDictionary.Values
                .GroupBy(node => node.Component)
                .Select(component => component.Select(node => node.ID).ToArray())
                .ToArray();
        }

        private void RunDFS(NodeWrap initial)
        {
            initial.Component = initial.ID;

            stack.Add(initial);

            while (stack.Any())
            {
                var node = stack.Last();
                stack.RemoveAt(stack.Count - 1);

                node.Node.AdjacentNodeIDs?.ForEach(adjacentId =>
                {
                    NodeWrap adjacent;
                    if (!nodesDictionary.TryGetValue(adjacentId, out adjacent))
                    { return; }

                    if (adjacent.Component == null)
                    {
                        adjacent.Component = node.Component;
                        stack.Add(adjacent);
                    }
                });
            }
        }

        class NodeWrap
        {
            public string ID => Node.ID;
            public GraphNode Node { get; set; }
            public string Component { get; set; }
        }
    }
}