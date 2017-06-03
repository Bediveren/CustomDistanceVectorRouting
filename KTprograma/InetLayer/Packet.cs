using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTprograma.InetLayer
{
    public class Packet
    {
        public int sentAtTick;
        public Router destination;
        public int hopcount;
        string message;

        public Packet(Router destination, string message)
        {
            hopcount = 0;
            this.destination = destination;
            this.message = message;
        }

        public void PrintMessage()
        {
            Console.WriteLine(message);
        }
    }
}
