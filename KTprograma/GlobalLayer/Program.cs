using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KTprograma.InetLayer;

namespace KTprograma
{
    class Program
    {
        static void Main(string[] args)
        {
            InetLayerController.CreateRouter(1);
            InetLayerController.CreateRouter(2);
            InetLayerController.CreateRouter(3);
            InetLayerController.CreateRouter(4);
            InetLayerController.CreateRouter(5);
            InetLayerController.CreateRouter(6);
            InetLayerController.CreateRouter(7);

            InetLayerController.CreateConnection(1, 2, 10);
            InetLayerController.CreateConnection(1, 3, 5);
            InetLayerController.CreateConnection(3, 4, 8);
            InetLayerController.CreateConnection(2, 4, 4);
            InetLayerController.CreateConnection(4, 5, 3);
            InetLayerController.CreateConnection(4, 6, 1);
            InetLayerController.CreateConnection(5, 7, 7);
            InetLayerController.CreateConnection(6, 7, 4);

            PrintCommands();
            int command;
            do
            {
                command = Int32.Parse(Console.ReadLine());
                switch (command)
                {
                    case 0:
                        PrintCommands();
                        break;
                    case 1:
                        Console.WriteLine("Enter router ID");
                        InetLayerController.CreateRouter(Int32.Parse(Console.ReadLine()));
                        break;
                    case 2:
                        Console.WriteLine("Enter router a ID, router b ID and weight between them");
                        InetLayerController.CreateConnection(Int32.Parse(Console.ReadLine()), Int32.Parse(Console.ReadLine()), Int32.Parse(Console.ReadLine()));
                        break;
                    case 3:
                        Console.WriteLine("Enter starting router, destination router, message");
                        InetLayerController.CreatePacket(Int32.Parse(Console.ReadLine()), Int32.Parse(Console.ReadLine()), Console.ReadLine());
                        break;
                    case 4:
                        InetLayerController.Tick();
                        break;
                    case 5:
                        Console.WriteLine("Enter router ID");
                        InetLayerController.PrintRoutingTable(Int32.Parse(Console.ReadLine()));
                        break;
                    case 6:
                        Console.WriteLine("Enter two routers you wish to cease connection with");
                        InetLayerController.RemoveConnection(Int32.Parse(Console.ReadLine()), Int32.Parse(Console.ReadLine()));
                        break;
                    case 7:
                        Console.WriteLine("Enter router ID");
                        InetLayerController.RemoveRouter(Int32.Parse(Console.ReadLine()));
                        break;
                    case 8:
                        Console.WriteLine("Enter router ID");
                        InetLayerController.SendRoutingTable(Int32.Parse(Console.ReadLine()));
                        break;
                    case 9:
                        break;
                    default:
                        Console.WriteLine("Unrecognised command");
                        break;

                }

            } while (command != 9) ;
        }

        static void PrintCommands()
        {
            Console.WriteLine("List of commands:");
            Console.WriteLine("0 - Print command list");
            Console.WriteLine("1 - Create new router");
            Console.WriteLine("2 - Create new connection");
            Console.WriteLine("3 - Create new packet");
            Console.WriteLine("4 - Tick");
            Console.WriteLine("5 - Print router routing table");
            Console.WriteLine("6 - Remove connection");
            Console.WriteLine("7 - Remove router");
            Console.WriteLine("8 - Router routing table signal");
            Console.WriteLine("9 - Exit");

        }
    }
}
