using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace saimmod3.Elements.Helper
{
    class Vocation
    {
        int liveTime = 0;


        public int LiveTime
        {
            get
            {
                return liveTime;
            }
        }


        public void Increment()
        {
            liveTime++;
        }
    }
}
