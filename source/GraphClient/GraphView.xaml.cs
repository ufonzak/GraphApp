using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Threading;
using System.Windows.Controls.Primitives;

namespace GraphClient
{
    /// <summary>
    /// Interaction logic for GraphView.xaml
    /// </summary>
    public partial class GraphView : UserControl
    {
        private GraphDataContext dataContext;

        private List<GraphNodeModel> selectedNodes = new List<GraphNodeModel>();

        private GraphLayout.IGraphLayout graphLayout;
        private DispatcherTimer layoutTimer;

        public GraphView()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            Loaded += GraphView_Loaded;

            dataContext = Kernel.Get<GraphDataContext>();
            DataContext = dataContext;

            graphLayout = Kernel.Get<GraphLayout.IGraphLayout>();

            layoutTimer = new DispatcherTimer();
            layoutTimer.Interval = TimeSpan.FromMilliseconds(100);
            layoutTimer.Tick += LayoutTimer_Tick;
        }

        private void GraphView_Loaded(object sender, RoutedEventArgs e)
        {
            btnLoadNodes.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        }

        private async void btnLoadNodes_Click(object sender, RoutedEventArgs e)
        {
            IsEnabled = false;

            selectedNodes.Clear();
            layoutTimer.Stop();

            await dataContext.LoadNodes();

            layoutTimer.Start();

            IsEnabled = true;
        }

        private async void btnFindPath_Click(object sender, RoutedEventArgs e)
        {
            if (selectedNodes.Count < 2)
            {
                MessageBox.Show("Please select two nodes.");
                return;
            }

            IsEnabled = false;

            await dataContext.FindPath(selectedNodes[0], selectedNodes[1]);

            IsEnabled = true;
        }

        private void LayoutTimer_Tick(object sender, EventArgs e)
        {
            bool runMore = graphLayout.RunInteration(dataContext.Nodes);
            PlaceComponents();

            if (!runMore)
            {
                layoutTimer.Stop();
            }
        }

        private void PlaceComponents()
        {
            double totalX = 0;
            double totalMaxY = 0;

            foreach (var component in dataContext.Nodes
                .GroupBy(node => node.Component)
                .OrderByDescending(component => component.Count()))
            {
                double minX = double.MaxValue, maxX = double.MinValue, minY = double.MaxValue, maxY = double.MinValue;

                foreach (var node in component)
                {
                    minX = Math.Min(minX, node.Position.X);
                    minY = Math.Min(minY, node.Position.Y);
                    maxX = Math.Max(maxX, node.Position.X);
                    maxY = Math.Max(maxY, node.Position.Y);
                }

                foreach (var node in component)
                {
                    node.Position = node.Position.Minus(new Point(minX - totalX, minY));
                }

                double width = maxX - minX + 100;
                double height = maxY - minY + 100;

                totalMaxY = Math.Max(totalMaxY, height);
                totalX += width;
            }

            canvas.Width = totalX;
            canvas.Height = totalMaxY;
        }

        private void Node_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string tag = (sender as FrameworkElement).Tag.ToString();
            GraphNodeModel model = dataContext.GetModel(tag);
            Select(model);
        }

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
