using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace KTprograma.InetLayer
{

    public class Router
    {
        public readonly int ID;

        private Packet buffer = null;

        private SortedList<Router, Row> Destinations;


        public Router(int ID)
        {
            Destinations = new SortedList<Router, Row>(new Comparer<Router>());
            this.ID = ID;
        }

        public void SendPacket()
        {
            if (buffer != null)
            {
                buffer.hopcount++;
                try
                {
                    if (buffer.sentAtTick != InetLayerController.CurrentTick)
                    {
                        Router nextDestination = Destinations[buffer.destination].ShortestPath.Key;
                        Console.WriteLine("Router:" + this.ID + " sending packet to router:" + nextDestination.ID);
                        buffer.sentAtTick = InetLayerController.CurrentTick;
                        nextDestination.RecievePacket(buffer);
                        buffer = null;
                    }

                }
                catch(Exception e)
                {
                    Console.WriteLine("The router:" + this.ID + " does not know a valid path to:" + buffer.destination.ID);
                }
            }
        }

        public void RecievePacket(Packet message)
        {
            if(this == message.destination)
            {
                message.PrintMessage();
            }
            else if (message.hopcount >= InetLayerController.MAXHOPSIZE)
            {
                Console.WriteLine("Packet can not be delivered to router:" + message.destination.ID);
            }
            else
            {
                buffer = message;
            }
        }

        public void AddDestination(Router router, Row connection)
        {
            if(Destinations.ContainsKey(router))
            {
                Destinations.Remove(router);
            }

            Destinations.Add(router, connection);
             
        }

        public void AddDestination(Router router, int weight)
        {
            Row table = new Row();
            table.AddConnection(router, weight);
            AddDestination(router, table);

        }

        public void AddDestination(KeyValuePair<Router, int> router, Router source)
        {

            if(router.Key != this)
            {
                if (!this.Destinations.ContainsKey(router.Key))
                {
                    Row newEntryRow = new Row();
                    newEntryRow.AddConnection(source, router.Value);
                    Destinations.Add(router.Key, newEntryRow);
                }
                else
                {
                    Destinations[router.Key].AddConnection(source, router.Value);
                }

            }
            else
            {
                Destinations[source].RemoveConnection(source);
                AddDestination(source, router.Value);
            }


        }

        public void RemoveDestination(Router router)
        {
            foreach (Row connection in Destinations.Values)
            {
                if (connection.Connections.ContainsKey(router))
                {
                        connection.Connections.Remove(router);
                        connection.AddConnection(router, InetLayerController.DOESNOTEXIST);

                }
            }
            cleanDirections();
            
        }


        private void TerminateDestination(Router router, Router source)
        {
            if(Destinations.ContainsKey(router))
            {
                Row element = Destinations[router];

                while(element.Connections.ContainsKey(source) && element.Connections[source] != -1)
                {
                    element.RemoveConnection(source);
                    element.AddConnection(source, InetLayerController.DOESNOTEXIST);
                }

            }

        }

        private void cleanDirections()
        {
            List<Router> emptyRouters = new List<Router>();

                foreach(KeyValuePair<Router, Row> element in Destinations)
                {
                    if(element.Value.Connections.Count == 0)
                    {
                        emptyRouters.Add(element.Key);
                    }
                }

                foreach(Router element in emptyRouters)
                {
                    Destinations.Remove(element);
                }

        }


        public void RecieveRoutingTable(RoutingTableMessage message)
        {
            foreach(var element in message.connections)
            {
                if(element.Value == InetLayerController.DOESNOTEXIST)
                {
                    TerminateDestination(element.Key, message.source);
                }
                else
                {
                    AddDestination(element, message.source);
                }
            }
            
        }


        private RoutingTableMessage FormLocalRoutingTable(Router destination)
        {
            RoutingTableMessage message = new RoutingTableMessage(this);

            foreach(KeyValuePair<Router, Row> dest in this.Destinations)
            {
                if (dest.Value.ShortestPath.Key != destination)
                {
                    if (dest.Value.ShortestPath.Value != InetLayerController.DOESNOTEXIST)
                    {
                        message.connections.Add(dest.Key, dest.Value.ShortestPath.Value + Destinations[destination].ShortestPath.Value);
                    }
                    else
                    {
                        message.connections.Add(dest.Key, dest.Value.ShortestPath.Value);
                    }
                }

            }
            return message;
        }

        public void SendRoutingTables()
        {
            
            foreach(Router dest in Destinations.Keys)
            {
                if (Destinations[dest].Connections.ContainsKey(dest) && Destinations[dest].Connections[dest] != InetLayerController.DOESNOTEXIST)
                {
                    RoutingTableMessage message = FormLocalRoutingTable(dest);
                    dest.RecieveRoutingTable(message);
                }
            }
        }

        public void Close()
        {
            foreach(var element in Destinations.Keys)
            {
                if (Destinations[element].Connections.ContainsKey(element) && Destinations[element].Connections[element] != InetLayerController.DOESNOTEXIST)
                {
                    element.RemoveDestination(this);
                    this.RemoveDestination(element);
                }

            }
        }



        public void PrintRoutingTable()
        {
            Console.WriteLine("From:" + this.ID);
            foreach (KeyValuePair<Router, Row> row in Destinations)
            {
                Console.WriteLine("Towards: " + row.Key.ID);
                row.Value.PrintRow();
            }
        }

    }
}
