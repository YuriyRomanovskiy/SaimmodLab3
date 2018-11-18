using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace saimmod3.Elements
{
    class Generator:Element
    {
        int period = 0;
        int currentTickCount = 0;
        bool canBeBlocked = false;
        bool isLocked = false;

        public Generator(int period, bool canBeBlocked)
        {
            this.period = period;
            currentTickCount = period;
            this.canBeBlocked = canBeBlocked;
            Element.OnBlockPrevious += Element_OnBlockPrevious;
        }


        public override string State
        {
            get
            {
                string result = "";
                if (isLocked)
                {
                    result = "x";
                }

                result = isLocked ? "x" : (currentTickCount + 1).ToString();

                return result;
            }
        }


        public override void ProcessTick(bool isFreeNextElement)
        {
            base.ProcessTick(isFreeNextElement);
#warning debug!
            //SendVocation();

            isFreeNextElement = true;

            if (reciever != null)
            {
                isFreeNextElement = !reciever.IsBusy;
            }


            bool isRecieverFree = isFreeNextElement;

            if (isLocked && isRecieverFree)
            {
                Unlock();
            }
            else if(isLocked && !isRecieverFree)
            {

            }
            else if (!isLocked && isRecieverFree)
            {
                if (currentTickCount == 0)
                {
                    SendVocation();
                }
                else
                {
                    currentTickCount--;
                }
            }
            else if (!isLocked && !isRecieverFree)
            {
                
                if (currentTickCount == 0)
                {
                    currentTickCount--;
                    Lock();
                }
                else
                {
                    currentTickCount--;
                }
            }

            //DebugLogs();
            

        }


        void DebugLogs()
        {
            //Debug.WriteLine("TICK " + currentTickCount);
            //Debug.WriteLine("IS LOCKED " + isLocked);
            //Debug.WriteLine("Gen "+State);
        }


        void Unlock()
        {
            isLocked = false;
            //currentTickCount = period;
            SendVocation();
        }


        void Lock()
        {
            isLocked = true;
            currentTickCount = -1;
        }


        public void SendVocation()
        {
            OnVocationCreated?.Invoke(this, reciever);
            currentTickCount = period;
            //Debug.WriteLine("VOCATION SEND");
        }


        void Element_OnBlockPrevious(Element sender, Element reciever)
        {
            //if (sender == this.reciever)
            //{
            //    Lock();
            //}
        }
    }
}
