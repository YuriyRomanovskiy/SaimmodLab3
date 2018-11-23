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
#warning shit
        List<Element> elementsInverse = new List<Element>();
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

        string State
        {
            get
            {
                sb.Clear();
                return generator.State + processor1.State + queue.State + processor2.State;
            }
        }


        public string MeanQueueLength
        {
            get
            {
                return ((float)generalQueueLemght / iterationsCOunt).ToString();
            }
        }



        public string MeanVocationsCount
        {
            get
            {
                return ((float)generalVocationsCount / iterationsCOunt).ToString();
            }
        }


        public string MeanTrafic
        {
            get
            {
                return ((float)elements[0].TraficCounter?.VocationsCount / iterationsCOunt).ToString();
            }
        }



        void Initialize(float probability1 = 0.0f, float probability2 = 0.0f)
        {
            traficCounter = new TraficCounter();
            generator = new Generator(1, true);
            processor1 = new Processor(probability1, true);
            queue = new Queue(1);
            processor2 = new Processor(probability2, true);

            generator.Init(null, processor1);
            processor1.Init(generator, queue);
            queue.Init(processor1, processor2);
            processor2.Init(queue, null, traficCounter);

            elements.Add(processor2);
            elements.Add(queue);
            elements.Add(processor1);
            elements.Add(generator);


            elementsInverse.Add(generator);
            elementsInverse.Add(processor1);
            elementsInverse.Add(queue);
            elementsInverse.Add(processor2);

        }


        public Manager(int iterationsCOunt = 100000, float probability1 = 0.0f, float probability2 = 0.0f)
        {
            Initialize(probability1, probability2);
            this.iterationsCOunt = iterationsCOunt;
        }


        
        public void ProcessTick()
        {
            elements.ForEach(item => item.ProcessTick(true));


            //processor2.ProcessTick(true);
            //queue.ProcessTick(!processor2.IsBusy);
            //processor1.ProcessTick(!queue.IsBusy);
            //generator.ProcessTick(!processor1.IsBusy);

            ///Debug.WriteLine(State);
            ///
            generalQueueLemght += queue.CurrentCapacity;
            generalVocationsCount += (processor1.State == "1") ? 1 : 0;
            generalVocationsCount += ( processor2.State == "1") ? 1 : 0;
            //generalVocationsCount += queue.CurrentCapacity;

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
            result += sum.ToString() + "\n-------";
            result += MeanQueueLength + "\n";
            result += MeanVocationsCount + "\n";
            result += MeanTrafic + "\n";



            return result;
        }


        public void Clear()
        {
            Initialize();
            statesCount.Clear();
        }
    }
}
