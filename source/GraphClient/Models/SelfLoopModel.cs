using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GraphClient
{
    public class SelfLoofModel : ModeBase
    {
        public GraphNodeModel Node { get; private set; }

        public Point Position => Node.Position;

        public SelfLoofModel(GraphNodeModel _node)
        {
            Node = _node;
            Node.PropertyChanged += Node_PropertyChanged;
        }

        private void Node_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Position")
            {
                RaisePropertyChanged("Position");
            }
        }
    }
}
