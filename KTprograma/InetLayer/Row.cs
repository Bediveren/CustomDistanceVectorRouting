using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTprograma.InetLayer
{
    
    public class Row
    {
        public KeyValuePair<Router, int> ShortestPath;

        public SortedList<Router, int> Connections;

        public Row()
        {
            Connections = new SortedList<Router, int>(new Comparer<Router>());
            ShortestPath = new KeyValuePair<Router, int>(null, InetLayerController.DOESNOTEXIST);
        }


        private void CalculateShortest()
        {
            ShortestPath = new KeyValuePair<Router, int>(null, InetLayerController.DOESNOTEXIST);
            foreach (KeyValuePair<Router, int> path in Connections)
            {
                if(path.Value < ShortestPath.Value || ShortestPath.Value == InetLayerController.DOESNOTEXIST)
                {
                    ShortestPath = path;
                }
            }
        }

        public void AddConnection(Router dest, int length)
        {
            if(Connections.ContainsKey(dest))
            {
                if (Connections[dest] > length || Connections[dest] == InetLayerController.DOESNOTEXIST)
                {
                    RemoveConnection(dest);
                }
                else
                {
                    return;
                }
            }
            Connections.Add(dest, length);
            CalculateShortest();
        }
        public void AddConnection(KeyValuePair<Router, int> connection)
        {
            AddConnection(connection.Key, connection.Value);
        }

        public void RemoveConnection(Router dest)
        {
            if(Connections.ContainsKey(dest))
            {
                Connections.Remove(dest);
            }
            CalculateShortest();
        }



        public void PrintRow()
        {
            foreach (KeyValuePair<Router, int> connection in Connections)
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.WriteLine("\t via: " + connection.Key.ID + " with length of: " + connection.Value);
                Console.ResetColor();
            }
        }

    }
}
