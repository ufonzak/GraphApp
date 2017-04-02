using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Timers;

namespace GraphClient
{
    /// <summary>
    /// Interaction logic for GraphView.xaml
    /// </summary>
    public partial class GraphView : UserControl
    {
        IGraphDataProvider dataProvider;

        private readonly Random random = new Random();

        public ObservableCollection<GraphNodeModel> Nodes { get; private set; } = new ObservableCollection<GraphNodeModel>();
        public ObservableCollection<EdgeModel> Edges { get; private set; } = new ObservableCollection<EdgeModel>();
        public ObservableCollection<SelfLoofModel> SelfLoops { get; private set; } = new ObservableCollection<SelfLoofModel>();

        public GraphView()
        {
            InitializeComponent();

            dataProvider = Kernel.Get<IGraphDataProvider>();

            Loaded += GraphView_Loaded;

            DataContext = this;

            layoutTimer = new Timer();
            layoutTimer.Interval = 100;
            layoutTimer.Elapsed += LayoutTimer_Elapsed;
        }

        private async void GraphView_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadNodes();
        }

        private async Task LoadNodes()
        {
            var nodes = await dataProvider.GetAllNodes();

            Nodes.Clear();
            Edges.Clear();
            SelfLoops.Clear();

            Dictionary<string, GraphNodeModel> viewsIdCache = new Dictionary<string, GraphNodeModel>();
            foreach (var node in nodes)
            {
                var nodeView = new GraphNodeModel(node)
                {
                    Position = new Point(random.NextDouble() * 1000, random.NextDouble() * 1000)
                };
                viewsIdCache.Add(node.ID, nodeView);
                Nodes.Add(nodeView);
            }

            foreach (var node in nodes)
            {
                GraphNodeModel view1 = viewsIdCache[node.ID];

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
                    if (!viewsIdCache.TryGetValue(adjacentId, out view2))
                    {
                        continue;
                    }

                    Edges.Add(new EdgeModel(view1, view2));
                }
            }

            layoutTimer.Start();
        }


        private const double REPULSION_FORCE = 3000.0;
        private const double ATRACTION_FORCE = 0.1;
        private const double DAMPING_COEF = 0.85;

        private void LayoutNodes()
        {
            double maxForce = 0;

            foreach (var node1 in Nodes)
            {
                Point force = new Point(0, 0);

                foreach (var node2 in Nodes.Where(node => node != node1))
                {
                    var difference = node1.Position.Minus(node2.Position);
                    var distance = difference.Size();

                    force = force.Plus(difference.Multi(REPULSION_FORCE / Math.Pow(distance, 2.0)));
                }

                foreach (var edge in node1.Edges)
                {
                    var difference = edge.P1.Minus(edge.P2);
                    if (edge.Node1 == node1)
                    {
                        difference = difference.Multi(-1);
                    }

                    force = force.Plus(difference.Multi(ATRACTION_FORCE));
                }

                node1.Position = node1.Position.Plus(force.Multi(DAMPING_COEF));
                maxForce = Math.Max(maxForce, force.Size());
            }

            if (maxForce < 0.1)
            {
                layoutTimer.Stop();
            }
        }

        private Timer layoutTimer;

        private void LayoutTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            LayoutNodes();
            OffsetGraph();
        }

        private void OffsetGraph()
        {
            double minX = double.MaxValue, maxX = double.MinValue, minY = double.MaxValue, maxY = double.MinValue;

            foreach (var node in Nodes)
            {
                minX = Math.Min(minX, node.Position.X);
                minY = Math.Min(minY, node.Position.Y);
                maxX = Math.Max(maxX, node.Position.X);
                maxY = Math.Max(maxY, node.Position.Y);
            }

            foreach (var node in Nodes)
            {
                node.Position = node.Position.Minus(new Point(minX, minY));
            }
        }
    }
}
