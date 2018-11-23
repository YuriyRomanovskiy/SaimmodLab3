using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace saimmod3.Elements.Helper
{
    class TraficCounter
    {
        int vocationsCount;

        public int VocationsCount
        {
            get
            {
                return vocationsCount;
            }
        }


        public void Increment()
        {
            vocationsCount++;
        }


        public void Reset()
        {
            vocationsCount = 0;
        }
    }
}
