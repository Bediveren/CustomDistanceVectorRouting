using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTprograma.InetLayer
{
    public class RoutingTableMessage
    {
        public Router source;
        public SortedList<Router, int> connections;

        public RoutingTableMessage(Router source)
        {
            this.source = source;
            connections = new SortedList<Router, int>(new Comparer<Router>());
        }
    }
}
