using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GraphClient.DataProvider;
using System.Collections.ObjectModel;
using System.ComponentModel;
using GraphServices.DTO;
using System.Windows.Threading;

namespace GraphClient
{
    class GraphDataContext
    {
        IGraphDataProvider dataProvider;

        private readonly Random random = new Random();

        public ObservableCollection<GraphNodeModel> Nodes { get; private set; } = new ObservableCollection<GraphNodeModel>();
        public ObservableCollection<EdgeModel> Edges { get; private set; } = new ObservableCollection<EdgeModel>();
        public ObservableCollection<SelfLoofModel> SelfLoops { get; private set; } = new ObservableCollection<SelfLoofModel>();

        Dictionary<string, GraphNodeModel> nodeModeIdCache2 = new Dictionary<string, GraphNodeModel>();

        public GraphNodeModel GetModel(string id)
        {
            GraphNodeModel model;
            if (!nodeModeIdCache2.TryGetValue(id, out model))
            {
                return null;
            }
            return model;
        }

        public GraphDataContext()
        {
            dataProvider = Kernel.Get<IGraphDataProvider>();
        }

        public async Task FindPath(GraphNodeModel node1, GraphNodeModel node2)
        {
            var path = await dataProvider.GetShortestPath(node1.ID, node2.ID);
            if (path == null)
            {
                MessageBox.Show("No path found.");
                return;
            }

            GraphNodeModel[] nodes = path.Select(nodeId => GetModel(nodeId)).ToArray();
            if (nodes.Any(n => n == null))
            {
                return;
            }

            foreach (var edge in Edges)
            {
                edge.Marked = false;
            }

            GraphNodeModel last = nodes[0];
            foreach (GraphNodeModel model in nodes.Skip(1))
            {
                var edge = last.Edges.FirstOrDefault(e => e.Node1 == model || e.Node2 == model);
                if (edge == null)
                {
                    return;
                }

                edge.Marked = true;
                last = model;
            }
        }

        public async Task LoadNodes()
        {
            var nodes = await dataProvider.GetAllNodes();
            var components = await dataProvider.GetGraphComponents();

            Nodes.Clear();
            nodeModeIdCache2.Clear();
            Edges.Clear();
            SelfLoops.Clear();

            foreach (var node in nodes)
            {
                var nodeView = new GraphNodeModel(node)
                {
                    Position = new Point(random.NextDouble() * 1000, random.NextDouble() * 1000)
                };
                nodeModeIdCache2.Add(node.ID, nodeView);
                Nodes.Add(nodeView);
            }

            foreach (var node in nodes)
            {
                GraphNodeModel view1 = GetModel(node.ID);

                foreach (var adjacentId in node.AdjacentNodeIDs)
                {
                    //self-loop
                    if (adjacentId == node.ID)
                    {
                        SelfLoops.Add(new SelfLoofModel(view1));
                        continue;
                    }

                    //resolve bi-directional edges
                    if (node.ID.CompareTo(adjacentId) > 0)
                    {
                        continue;
                    }

                    GraphNodeModel view2;
                    if ((view2 = GetModel(adjacentId)) == null)
                    {
                        continue;
                    }

                    Edges.Add(new EdgeModel(view1, view2));
                }
            }

            foreach (var component in components)
            {
                foreach (var nodeId in component)
                {
                    GetModel(nodeId).Component = component[0];
                }
            }
        }
    }
}
