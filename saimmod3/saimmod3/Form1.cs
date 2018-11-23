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

        Manager mng;//= new Manager(100000);

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int iterrationsCount = GetInputDataInt(iterrationsTextBox, 100000);
            float probability1 = GetInputDataFloat(firstProbabilitytextBox, 0.4f);
            float probability2 = GetInputDataFloat(secondProbabilitytextBox, 0.5f);

            mng = new Manager(100000, probability1, probability2);
            mng.ProcessManyTicks();
            richTextBox1.Text = mng.PrintAll();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            mng?.Clear();
            richTextBox1.Text = "";
        }


        float GetInputDataFloat(TextBox textBox, float defaultValue = 0f)
        {
            float result = defaultValue;

            if (!Single.TryParse(textBox.Text, out result))
            {
                result = defaultValue;
                textBox.Text = defaultValue.ToString();
            }

            return result;
        }


        int GetInputDataInt(TextBox textBox, int defaultValue = 0)
        {
            int result = defaultValue;

            if (!Int32.TryParse(textBox.Text, out result))
            {
                result = defaultValue;
                textBox.Text = defaultValue.ToString();
            }
            
            return result;
        }
    }
}
