using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Youtube_Media_Player
{
    public partial class SearchPage : Form
    {
        mainform mainForm;
        
        public SearchPage()
        {
            InitializeComponent();
            //textBox1.Text = "Load";
            //textBox1.Controls.Add(vScrollBar1);
            //mainform
            //vScrollBar1.Controls.Add(textBox1);
            //vScrollBar1.

            //


            //mainForm = new mainform();
            
            
        }

        private void SearchPage_Load(object sender, EventArgs e)
        {

        }
        

        //載入mainform到SearchPage
        public void setForm(mainform f)
        {
            mainForm = f;
        //    Console.Write(f.x+1);
            this.textBox1.Text = "123";
            //Console.Write("FF00000");
         //   textBox1.Text = "132"+f.x;




        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }



    }
}
