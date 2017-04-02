using GraphServices.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GraphClient
{
    public class GraphNodeModel : ModeBase
    {
        private Point position;
        public Point Position
        {
            get { return position; }
            set
            {
                position = value;
                RaisePropertyChanged("Position");
            }
        }

        public List<EdgeModel> Edges { get; private set; } = new List<EdgeModel>();

        public string Label => node.Label;
        public string ID => node.ID;

        private GraphNode node;

        public GraphNodeModel(GraphNode _node)
        {
            node = _node;
        }
    }
}
