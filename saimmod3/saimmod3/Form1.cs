using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using saimmod3.Elements;

namespace saimmod3
{
    public partial class Form1 : Form
    {
        Generator generator = new Generator(1, true);
        Processor processor1 = new Processor(0.2f, true);
        Queue queue = new Queue(1);
        Processor processor2 = new Processor(0.8f, true);

        Manager mng = new Manager(100);

        public Form1()
        {
            InitializeComponent();
            generator.Init(null, processor1);
            processor1.Init(generator, queue);
            queue.Init(processor1, processor2);
            processor2.Init(queue, null);
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //processor2.ProcessTick(true);
            //queue.ProcessTick(!processor2.IsBusy);
            //processor1.ProcessTick(!queue.IsBusy);
            //generator.ProcessTick(!processor1.IsBusy);

          mng.ProcessManyTicks();

            //Debug.WriteLine(generator.State + processor1.State + queue.State + processor2.State);
            //Debug.WriteLine("----");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            mng.PrintAll();
        }
    }
}
