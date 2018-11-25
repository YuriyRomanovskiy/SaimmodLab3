using saimmod3.Elements.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace saimmod3.Elements
{
    class Queue : Element
    {
        int capacity = 0;
        int currentCapacity = 0;

        public int CurrentCapacity
        {
            get
            {
                return currentCapacity;
            }
        }


        //bool isBusy = false;


        public override string State
        {
            get
            {
                return currentCapacity.ToString();
            }
        }


        public override bool IsBusy
        {
            get
            {
                return currentCapacity >= capacity;
            }
        }


        public Queue(int capacity)
        {
            this.capacity = capacity;
            Element.OnVocationCreated += Element_OnVocationCreated;
        }


        public override void ProcessTick(bool isFreeNextElement)
        {
            base.ProcessTick(isFreeNextElement);

            VocationLiveTimeIncrement();

            isFreeNextElement = true;

            if (reciever != null)
            {
                isFreeNextElement = !reciever.IsBusy;
            }

            if (isFreeNextElement && currentCapacity > 0)
            {
                currentCapacity--;

                OnVocationCreated?.Invoke(this, reciever, vocation);
                this.vocation = null;


                if (counter != null)
                {
                    counter.Increment();
                }

            }

            if (currentCapacity == capacity)
            {
                isBusy = true;
            }
            else
            {
                isBusy = false;
            }
        }


        void Element_OnVocationCreated(Element sender, Element rec, Vocation vocation)
        {

            

            if (sender == this.sender)
            {
                if (this.vocation != null)
                {
                    Debug.WriteLine("ssssppp");
                }
                this.vocation = vocation;

                
                if (((Processor)this.reciever).IsBusy)
                {
                    if (currentCapacity >= capacity)
                    {
                        isBusy = true;
                    }

                    if (!isBusy)
                    currentCapacity++;
                }
                else
                {
                    OnVocationCreated?.Invoke(this, this.reciever, vocation);

                    this.vocation = null;

                    if (counter != null)
                    {
                        counter.Increment();
                    }
                }

               
            }
           
        }
    }
}
