﻿using saimmod3.Elements.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace saimmod3.Elements
{
    class Processor: Element
    {
        float probability = 0f;
        bool canBlock = false;
        bool isBLocked = false;
        Random rand;

        #region Properties

        public override string State
        {
            get
            {
                string result = isBusy ? "1" : "0";
                if (isBLocked)
                {
                    result = "x";
                }

                return result;
            }
        }


        public float Probability
        {
            get
            {
                return probability;
            }
        }


        public override bool IsBusy
        {
            get
            {
                return isBusy;
            }
        }


        bool IsBlocked
        {
            get
            {
                return isBLocked;
            }
        }

        #endregion


        public Processor(float probability, bool canBlock)
        {
            this.probability = probability;
            this.canBlock = canBlock;
            Element.OnVocationCreated += Element_OnVocationCreated;
            rand = CustomRandom.Instance.Rand;
        }


        public bool TrySetVocation()
        {
            bool result = false;
           if (isBusy)
            {
                result = false;
            }
           else
            {
                result = true;
                isBusy = true;
            }

            return result;
        }


        bool IsVocationProcessed()
        {
           
            bool result = false;

            double randomValue = rand.NextDouble();

            if (Convert.ToSingle(rand.NextDouble()) <= (1f-probability))
            {
                result = true;
            }

            return result;
        }


        public override void ProcessTick(bool isFreeNextElement)
        {
            
            base.ProcessTick(isFreeNextElement);
            isFreeNextElement = true;

            

            VocationLiveTimeIncrement();

            if (reciever != null)
            {
                isFreeNextElement = !reciever.IsBusy;
            }

            if (isBusy)
            {
                if (isBLocked && isFreeNextElement)
                {
                    isBLocked = false;
                    isBusy = false;

                    OnVocationCreated?.Invoke(this, reciever, vocation);
                    vocation = null;


                    IsProcessed = true;
                    return;

                }

                if (!isBLocked)
                {
                    if (IsVocationProcessed())
                    {
                        if (!isFreeNextElement)
                        {
                            isBLocked = true;
                        }
                        else
                        {
                            isBusy = false;

                            OnVocationCreated?.Invoke(this, reciever, vocation);
                            vocation = null;

                            if (counter != null)
                            {
                                counter.Increment();
                            }
                        }
                    }
                    else
                    {
                        if (sender != null && OnBlockPrevious != null)
                        {
                            OnBlockPrevious(this, sender);
                        }
                    }
                }

            }
            else
            {
                //isBusy = true;
            }

            IsProcessed = true;
        }


        #region Events handlers

        void Element_OnVocationCreated(Element sender, Element reciever, Vocation vocation)
        {
            if (sender == this.sender)
            {

                if (this.vocation != null)
                {
                    Debug.WriteLine("ssssppp");
                }


                this.vocation = vocation;

                
                if (isBusy)
                {
                    Debug.WriteLine("ssssssss");
                }
                isBusy = true;
            }
        }

        #endregion
    }
}
