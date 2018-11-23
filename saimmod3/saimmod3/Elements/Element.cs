using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using saimmod3.Elements.Helper;


namespace saimmod3.Elements
{
    class Element
    {

        public static Action<Element, Element> OnVocationCreated;
        public static Action<Element, Element> OnBlockPrevious;

        protected Element sender;
        protected Element reciever;
        protected TraficCounter counter;

        protected bool isBusy = false;


        public virtual TraficCounter TraficCounter
        {
            get
            {
                return counter;
            }
        }


        public virtual bool IsBusy
        {
            get
            {
               return isBusy;
            }
        }


        public virtual string State
        {
            get
            {
                return "";
            }
        }


        public bool IsProcessed
        {
            get;set;
        }


        public virtual void ProcessTick(bool isFreeNextElement)
        {
            
        }


        public virtual void Init(Element sender, Element reciever, TraficCounter counter = null)
        {
            IsProcessed = false;
            this.sender = sender;
            this.reciever = reciever;
            this.counter = counter;
        }
    }
}
