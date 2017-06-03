using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections;
using KTprograma.InetLayer;


namespace KTprograma
{

    public static class InetLayerController
    {
        
        public static Dictionary<int, Router> networkRouters = new Dictionary<int, Router>();
        public const int DOESNOTEXIST = -1;
        public const int MAXHOPSIZE = 15;
        public static int CurrentTick = 0;

        public static void Tick()
        {
            foreach(Router router in networkRouters.Values)
            {
                router.SendPacket();
            }
            CurrentTick++;
        }

        public static void CreateRouter(int ID)
        {
            networkRouters.Add(ID, new Router(ID));
        }

        public static void CreateConnection(int aID, int bID, int weight)
        {
            networkRouters[aID].AddDestination(networkRouters[bID], weight);
            networkRouters[bID].AddDestination(networkRouters[aID], weight);
        }

        public static void RemoveRouter(int ID)
        {
            networkRouters[ID].Close();
            networkRouters.Remove(ID);

        }

        public static void RemoveConnection(int a, int b)
        {
            networkRouters[a].RemoveDestination(networkRouters[b]);
            networkRouters[b].RemoveDestination(networkRouters[a]);
        }

        public static void PrintRoutingTable(int ID)
        {
            networkRouters[ID].PrintRoutingTable();
        }

        public static void SendRoutingTable(int ID)
        {
            networkRouters[ID].SendRoutingTables();
        }

        public static void CreatePacket(int source, int destination, string message)
        {
            Packet packet = new Packet(networkRouters[destination], message);
            networkRouters[source].RecievePacket(packet);
        }

    }

    

}
