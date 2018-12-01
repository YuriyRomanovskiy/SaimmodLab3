using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using saimmod3.Elements;
using saimmod3.Elements.Helper;

namespace saimmod3
{
    class Manager
    {
        List<Element> elements = new List<Element>();

        List<Element> elementsInverse = new List<Element>();
        List<Vocation> vocations = new List<Vocation>();
        Dictionary<string, int> statesCount = new Dictionary<string, int>();

        TraficCounter traficCounter;// = new TraficCounter();
        Generator generator;// = new Generator(1, true);
        Processor processor1;// = new Processor(.4f, true);
        Queue queue;// = new Queue(1);
        Processor processor2;// = new Processor(.5f, true);

        StringBuilder sb = new StringBuilder();
        int stateCount = 0;
        string state;
        int iterationsCOunt = 0;

        int generalVocationsCount = 0;
        int generalQueueLemght = 0;

        public string State
        {
            get
            {
                sb.Clear();
                return generator.State + processor1.State + queue.State + processor2.State;
            }
        }


        public float MeanQueueLength
        {
            get
            {
                return ((float)generalQueueLemght / iterationsCOunt);
            }
        }



        public float MeanVocationsCount
        {
            get
            {
                return ((float)generalVocationsCount / iterationsCOunt);
            }
        }


        public float MeanTrafic
        {
            get
            {
                return ((float)elements[0].TraficCounter?.VocationsCount / iterationsCOunt);
            }
        }



        void Initialize(float probability1 = 0.0f, float probability2 = 0.0f)
        {
            traficCounter = new TraficCounter();
            generator = new Generator(1, true);
            processor1 = new Processor(probability1, true);
            queue = new Queue(1);
            processor2 = new Processor(probability2, true);

            generator.Init(null, processor1, manager:this);
            processor1.Init(generator, queue, manager: this);
            queue.Init(processor1, processor2, null, manager: this);
            processor2.Init(queue, null, traficCounter, manager: this);

            elements.Add(processor2);
            elements.Add(queue);
            elements.Add(processor1);
            elements.Add(generator);


            elementsInverse.Add(generator);
            elementsInverse.Add(processor1);
            elementsInverse.Add(queue);
            elementsInverse.Add(processor2);


            Generator.OnVocationCreated += Generator_OnVocationCreated;

        }


        public Manager(int iterationsCOunt = 100000, float probability1 = 0.0f, float probability2 = 0.0f)
        {
            Initialize(probability1, probability2);
            this.iterationsCOunt = iterationsCOunt;
        }


        
        public void ProcessTick()
        {
            elements.ForEach(item => item.ProcessTick(true));


            //Debug.WriteLine(State);
            //vocations.ForEach(item => Debug.WriteLine(item.LiveTime));
            //Debug.WriteLine("-------");


            //processor2.ProcessTick(true);
            //queue.ProcessTick(!processor2.IsBusy);
            //processor1.ProcessTick(!queue.IsBusy);
            //generator.ProcessTick(!processor1.IsBusy);

            ///Debug.WriteLine(State);
            ///
            generalQueueLemght += queue.CurrentCapacity;

            generalVocationsCount += ((processor1.State == "x") || (processor1.State == "1")) ? 1 : 0;
            generalVocationsCount += ((processor2.State == "x") || (processor2.State == "1")) ? 1 : 0;
            generalVocationsCount += queue.CurrentCapacity;

            state = State;
            if (statesCount.TryGetValue(state, out stateCount))
            {
                //stateCount++;
                statesCount[state]++;
            }
            else
            {
                statesCount.Add(state, 1);
            }
        }


        public void ProcessManyTicks()
        {
            for (int i = 0; i < iterationsCOunt; i++)
            {
                ProcessTick();

            }
            Debug.WriteLine("fin");
        }


        public string PrintAll()
        {
            string result = "";
            float sum = 0f;
            foreach(var item in statesCount)
            {
                result += item.Key + " " + ((float)item.Value / iterationsCOunt).ToString() + "\n";
                sum += ((float)item.Value / iterationsCOunt);
            }
            result += sum.ToString() + "\n-------\n";
            result += "Loch "+ MeanQueueLength.ToString() + "\n";
            result += "Ls " + MeanVocationsCount.ToString() + "\n";
            result += "A " + MeanTrafic.ToString() + "\n";
            //result += "W " +(MeanVocationsCount / MeanTrafic ).ToString() + "\n";
            result += "W" + W.ToString();


            return result;
        }


        float W
        {
            get
            {
                float sum = 0f;
                vocations.ForEach(item => sum += item.LiveTime);
                return sum / vocations.Count;
            }
        }


        public void Clear()
        {
            Initialize();
            statesCount.Clear();
        }


        void Generator_OnVocationCreated(Element sender, Element rec, Vocation vocation)
        {
            if (sender == processor2)
            {
                vocations.Add(vocation);
                //Debug.WriteLine(vocation.LiveTime);
            }
        }
    }
}
