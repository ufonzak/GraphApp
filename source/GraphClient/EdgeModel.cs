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
    public class EdgeModel : ModeBase
    {
        public GraphNodeModel Node1 { get; private set; }
        public GraphNodeModel Node2 { get; private set; }

        public Point P1 => Node1.Position;
        public Point P2 => Node2.Position;

        private bool marked;
        public bool Marked
        {
            get { return marked; }
            set
            {
                marked = value;
                RaisePropertyChanged("Marked");
            }
        }

        public EdgeModel(GraphNodeModel _node1, GraphNodeModel _node2)
        {
            Node1 = _node1;
            Node2 = _node2;

            Node1.Edges.Add(this);
            Node2.Edges.Add(this);

            Node1.PropertyChanged += Node_PropertyChanged;
            Node2.PropertyChanged += Node_PropertyChanged;
        }

        private void Node_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Position")
            {
                if (sender == Node1)
                {
                    RaisePropertyChanged("P1");
                }
                else {
                    RaisePropertyChanged("P2");
                }
            }
        }
    }

}
