using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace TooCalculator
{
    

    public partial class PrimaryWindow : Form
    {

        public PrimaryWindow()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }
        private void button1_Click(object sender, EventArgs e)
        {

           // Console.WriteLine(Program.validUserName);

                pictureBox1.Enabled = true;
                backgroundWorker1.RunWorkerAsync();
                pictureBox1.Visible = true;
            //clear List
                Program.info.Clear();

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
           
        }
        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }
        private void label1_Click(object sender, EventArgs e)
        {
        }
        private void label3_Click(object sender, EventArgs e)
        {

        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Program.getMembershipId(textBox1.Text);

        }
        // run the retriving of data in Background thread to allow for loading symbol!
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Show warning symbol if username is invalid!
            if (Program.validUserName == false)
            {
                WarningWindow warning = new WarningWindow();
                warning.ShowDialog();

                pictureBox1.Visible = false;
            }
            
            //update text!
            if (Program.validUserName) { 
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";

            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";

            textBox9.Text = "";
            textBox10.Text = "";
            textBox11.Text = "";

            
            textBox3.Text = Program.info[0].name;
            textBox4.Text = Program.info[0].cName;
            textBox5.Text = Program.info[0].kd;

            textBox6.Text = Program.info[1].name;
            textBox7.Text = Program.info[1].cName;
            textBox8.Text = Program.info[1].kd;

            textBox9.Text = Program.info[2].name;
            textBox10.Text = Program.info[2].cName;
            textBox11.Text = Program.info[2].kd;

            pictureBox1.Visible = false;
            
        }

        }


    }
}
