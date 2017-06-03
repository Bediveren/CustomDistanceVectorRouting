using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTprograma.InetLayer
{
    class Comparer<T> : IComparer<T> where T : Router
    {
        public int Compare(T x, T y)
        {
            if (x.ID > y.ID) return 1;
            else if (x.ID < y.ID) return -1;
            else return 0;
        }
    }
}
