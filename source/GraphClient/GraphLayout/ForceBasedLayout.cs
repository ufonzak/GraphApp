using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GraphClient.GraphLayout
{
    class ForceBasedLayout : IGraphLayout
    {
        private const double REPULSION_FORCE = 10000.0;
        private const double ATRACTION_FORCE = 0.1;
        private const double DAMPING_COEF = 0.95;

        public bool RunInteration(IList<GraphNodeModel> nodes)
        {
            double maxForce = 0;

            foreach (var node1 in nodes)
            {
                Point force = new Point(0, 0);

                foreach (var node2 in nodes.Where(node => node != node1 && node.Component == node1.Component))
                {
                    var difference = node1.Position.Minus(node2.Position);
                    var distance = difference.Size();

                    force = force.Plus(difference.Multi(REPULSION_FORCE / Math.Pow(distance, 2.0) / nodes.Count));
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

            return maxForce >= 2.0;
        }
    }
}
