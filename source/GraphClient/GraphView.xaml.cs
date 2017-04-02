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
using System.Windows.Threading;

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

        Dictionary<string, GraphNodeModel> nodeModeIdCache = new Dictionary<string, GraphNodeModel>();

        public GraphView()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            dataProvider = Kernel.Get<IGraphDataProvider>();

            Loaded += GraphView_Loaded;

            DataContext = this;

            layoutTimer = new DispatcherTimer();
            layoutTimer.Interval = TimeSpan.FromMilliseconds(100);
            layoutTimer.Tick += LayoutTimer_Tick;
        }

        private async void GraphView_Loaded(object sender, RoutedEventArgs e)
        {
            IsEnabled = false;
            await LoadNodes();
            IsEnabled = true;
        }

        private async void btnLoadNodes_Click(object sender, RoutedEventArgs e)
        {
            IsEnabled = false;
            await LoadNodes();
            IsEnabled = true;
        }

        private async void btnFindPath_Click(object sender, RoutedEventArgs e)
        {
            IsEnabled = false;
            await FindPath();
            IsEnabled = true;
        }

        private async Task FindPath()
        {
            if (selectedNodes.Count < 2)
            {
                MessageBox.Show("Please select two nodes.");
                return;
            }

            var path = await dataProvider.GetShortestPath(selectedNodes[0].ID, selectedNodes[1].ID);
            if (path == null)
            {
                MessageBox.Show("No path found.");
                return;
            }

            GraphNodeModel[] nodes;
            try
            {
                nodes = path.Select(nodeId => nodeModeIdCache[nodeId]).ToArray();
            }
            catch (KeyNotFoundException)
            {
                return;
            }

            foreach (var edge in Edges)
            {
                edge.Marked = false;
            }

            GraphNodeModel last = nodes[0];
            foreach(GraphNodeModel model in nodes.Skip(1))
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

        private async Task LoadNodes()
        {
            var nodes = await dataProvider.GetAllNodes();

            Nodes.Clear();
            nodeModeIdCache.Clear();
            Edges.Clear();
            SelfLoops.Clear();

            foreach (var node in nodes)
            {
                var nodeView = new GraphNodeModel(node)
                {
                    Position = new Point(random.NextDouble() * 1000, random.NextDouble() * 1000)
                };
                nodeModeIdCache.Add(node.ID, nodeView);
                Nodes.Add(nodeView);
            }

            foreach (var node in nodes)
            {
                GraphNodeModel view1 = nodeModeIdCache[node.ID];

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
                    if (!nodeModeIdCache.TryGetValue(adjacentId, out view2))
                    {
                        continue;
                    }

                    Edges.Add(new EdgeModel(view1, view2));
                }
            }

            layoutTimer.Start();
        }

        #region layout
        //TODO: refactor to class

        private DispatcherTimer layoutTimer;

        private void LayoutTimer_Tick(object sender, EventArgs e)
        {
            LayoutNodes();
            OffsetGraph();
        }

        private const double REPULSION_FORCE = 10000.0;
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

                    force = force.Plus(difference.Multi(REPULSION_FORCE / Math.Pow(distance, 2.0) / Nodes.Count));
                }

                foreach (var edge in node1.Edges)
                {
                    var difference = edge.P1.Minus(edge.P2);
                    if (edge.Node1 == node1)
                    {
                        difference = difference.Multi(-1);
                    }

                    force = force.Plus(difference.Multi(ATRACTION_FORCE / node1.Edges.Count));
                }

                node1.Position = node1.Position.Plus(force.Multi(DAMPING_COEF));
                maxForce = Math.Max(maxForce, force.Size());
            }

            if (maxForce < 0.1)
            {
                layoutTimer.Stop();
            }
        }
        #endregion

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

            double width = maxX - minX + 100;
            double height = maxY - minY + 100;

            canvas.Width = width;
            canvas.Height = height;
        }

        private void Node_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string tag = (sender as FrameworkElement).Tag.ToString();
            GraphNodeModel model = nodeModeIdCache[tag];
            Select(model);
        }

        private List<GraphNodeModel> selectedNodes = new List<GraphNodeModel>();
        private void Select(GraphNodeModel node)
        {
            if (node.Selected)
            {
                node.Selected = false;
                selectedNodes.Remove(node);
                return;
            }

            node.Selected = true;
            selectedNodes.Add(node);

            if (selectedNodes.Count > 2)
            {
                selectedNodes[0].Selected = false;
                selectedNodes.RemoveAt(0);
            }
        }
    }
}
