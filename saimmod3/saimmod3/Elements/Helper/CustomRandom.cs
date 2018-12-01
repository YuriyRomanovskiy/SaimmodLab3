using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace saimmod3.Elements.Helper
{
    class CustomRandom
    {
        static CustomRandom instance;

        Random rand = null;

        public static CustomRandom Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CustomRandom();
                }
                return instance;
            }
        }


        public Random Rand
        {
            get
            {
                if (rand == null)
                {
                    rand = new Random();
                    Debug.WriteLine("new rand");
                }

                return rand;
            }
        }

    }
}
