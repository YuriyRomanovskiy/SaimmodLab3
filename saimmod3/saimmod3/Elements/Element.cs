using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace saimmod3.Elements
{
    class Element
    {

        public static Action<Element, Element> OnVocationCreated;
        public static Action<Element, Element> OnBlockPrevious;

        protected Element sender;
        protected Element reciever;

        protected bool isBusy = false;


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


        public virtual void Init(Element sender, Element reciever)
        {
            IsProcessed = false;
            this.sender = sender;
            this.reciever = reciever;
        }
    }
}
