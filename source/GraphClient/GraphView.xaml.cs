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

namespace GraphClient
{
    /// <summary>
    /// Interaction logic for GraphView.xaml
    /// </summary>
    public partial class GraphView : UserControl
    {
        IGraphDataProvider dataProvider;

        public ObservableCollection<GraphNodeView> Nodes { get; private set; } = new ObservableCollection<GraphNodeView>();
        public ObservableCollection<EdgeView> Edges { get; private set; } = new ObservableCollection<EdgeView>();

        public GraphView()
        {
            InitializeComponent();

            dataProvider = Kernel.Get<IGraphDataProvider>();

            Loaded += GraphView_Loaded;

            DataContext = this;
        }

        private readonly Random random = new Random();

        private async void GraphView_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadNodes();
        }

        private async Task LoadNodes()
        {
            var nodes = await dataProvider.GetAllNodes();

            Nodes.Clear();
            Edges.Clear();

            Dictionary<string, GraphNodeView> viewsIdCache = new Dictionary<string, GraphNodeView>();
            foreach (var node in nodes)
            {
                var nodeView = new GraphNodeView(node)
                {
                    Position = new Point(random.NextDouble() * 600, random.NextDouble() * 600)
                };
                viewsIdCache.Add(node.ID, nodeView);
                Nodes.Add(nodeView);
            }

            foreach (var node in nodes)
            {
                GraphNodeView view1 = viewsIdCache[node.ID];

                foreach (var adjacentId in node.AdjacentNodeIDs)
                {
                    //TODO: solve self-loop
                    //resolve bi-directional edges
                    if (node.ID.CompareTo(adjacentId) >= 0)
                    {
                        continue;
                    }

                    GraphNodeView view2;
                    if (!viewsIdCache.TryGetValue(adjacentId, out view2))
                    {
                        continue;
                    }

                    Edges.Add(new EdgeView(view1, view2));
                }
            }
        }
    }

    public class EdgeView : INotifyPropertyChanged
    {
        private GraphNodeView node1;
        private GraphNodeView node2;

        public event PropertyChangedEventHandler PropertyChanged;

        public Point P1 => node1.Position;
        public Point P2 => node2.Position;

        public EdgeView(GraphNodeView _node1, GraphNodeView _node2)
        {
            node1 = _node1;
            node2 = _node2;
        }

        public void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }

    public class GraphNodeView : INotifyPropertyChanged
    {
        public Point Position { get; set; }

        public string Label => node.Label;
        public string ID => node.ID;

        public event PropertyChangedEventHandler PropertyChanged;

        private GraphNode node;

        public GraphNodeView(GraphNode _node)
        {
            node = _node;
        }

        public void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
